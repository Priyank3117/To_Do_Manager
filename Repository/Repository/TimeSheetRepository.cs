using Entities.Data;
using Entities.Models;
using Entities.ViewModels.TimeSheet;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Repository.Interface;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;


namespace Repository.Repository
{
    public class TimeSheetRepository : ITimeSheetRepository
    {

        private readonly ToDoManagerDBContext _db;

        public TimeSheetRepository(ToDoManagerDBContext toDoManagerDBContext)
        {
            _db = toDoManagerDBContext;
        }
        public async void AddTimeSheetData(TimeSheetViewModel timeSheetViewModel)
        {

            //first we added the data of the timesheet input log
            TimeSheetInputLog timeSheetInputLog = new TimeSheetInputLog();

            if (timeSheetViewModel.FileName == null)
            {
                IFormFile file = timeSheetViewModel.UploadedFile;
                timeSheetInputLog.FileName = file.FileName;
            }
            else
            {
                timeSheetInputLog.FileName = timeSheetViewModel.FileName;
            }
            string[] splitProjectTypeAndId = timeSheetViewModel.ProjectType.Split(',');
            timeSheetInputLog.ProjectType = splitProjectTypeAndId[0];
            _db.TimeSheetInputLogs.Add(timeSheetInputLog);
            _db.SaveChanges();

            //now save the data of time sheet details
            using (var workbook = new ClosedXML.Excel.XLWorkbook(timeSheetViewModel.UploadedFile.OpenReadStream()))
            {
                //Lets assume the First Worksheet contains the data
                var worksheet = workbook.Worksheet(1);

                //Lets assume first row contains the header, so skip the first row
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                Regex urlRegex = new Regex(@"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-]*)");

                //Loop Through all the Rows except the first row which contains the header data
                foreach (var row in rows)
                {
                    //get the data from the timesheet deatil table and check whether in the table or not

                    var list = _db.TimeSheetDetails.ToList();

                    if (list.Count > 0)
                    {
                        var timeSheetDetail = new TimeSheetDetails();

                        timeSheetDetail.TimeSheetInputLogId = timeSheetInputLog.TimeSheetInputLogID;
                        timeSheetDetail.ProjectType = row.Cell(3).GetValue<string>();
                        timeSheetDetail.ProjectCode = row.Cell(4).GetValue<string>();
                        timeSheetDetail.Task = row.Cell(5).GetValue<string>();
                        timeSheetDetail.Note = row.Cell(6).GetValue<string>();
                        Match match = urlRegex.Match(timeSheetDetail.Note);

                        if (match.Success)
                        {
                            timeSheetDetail.Url = match.Value;
                        }
                        else
                        {
                            timeSheetDetail.Url = row.Cell(15).GetValue<string>();
                        }

                        var isPresent = list.Any(record => (record.Task == timeSheetDetail.Task) && (record.ProjectType == timeSheetDetail.ProjectType) &&
                        (record.ProjectCode == timeSheetDetail.ProjectCode) && (record.Note == timeSheetDetail.Note) &&
                       (record.Url == timeSheetDetail.Url));

                        if (isPresent)
                        {
                            continue;
                        }
                        else
                        {
                            if (timeSheetDetail.ProjectType.Contains(timeSheetInputLog.ProjectType))
                            {
                                _db.TimeSheetDetails.Add(timeSheetDetail);
                            }
                        }
                    }
                    else
                    {
                        var timeSheetDetail = new TimeSheetDetails();

                        timeSheetDetail.TimeSheetInputLogId = timeSheetInputLog.TimeSheetInputLogID;
                        timeSheetDetail.ProjectType = row.Cell(3).GetValue<string>();
                        timeSheetDetail.ProjectCode = row.Cell(4).GetValue<string>();
                        timeSheetDetail.Task = row.Cell(5).GetValue<string>();
                        timeSheetDetail.Note = row.Cell(6).GetValue<string>();
                        timeSheetDetail.Url = row.Cell(15).GetValue<string>();
                        //string date = row.Cell(5).GetValue<string>();
                        //timeSheetDetails. = DateOnly.FromDateTime(DateTime.Parse(date));
                        //Add the Employee to the List of Employees
                        if (timeSheetDetail.ProjectType.Contains(timeSheetInputLog.ProjectType))
                        {
                            _db.TimeSheetDetails.Add(timeSheetDetail);

                        }
                    }
                }

                _db.SaveChanges();
            }
            var timeSheetDetailData = _db.TimeSheetDetails.Where(s => s.TimeSheetInputLogId == timeSheetInputLog.TimeSheetInputLogID).ToList();
            AsanaTOHtmlDoc(timeSheetInputLog.TimeSheetInputLogID, splitProjectTypeAndId[1], timeSheetDetailData);
        }

        public void AsanaTOHtmlDoc(long TimeSheetInputLogID, string projectId, List<TimeSheetDetails> timesheetdetails)
        {
            var viewModel = new AsanaViewModel();
            var matchedViewModel = new AsanaViewModel();

            using (var _httpClient = new HttpClient())
            {
                // Set the Bearer token in the request headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "2/1207683751267451/1207686063568260:d7c2960bcaba823b350bd3531c58d7eb");

                string url = $"https://app.asana.com/api/1.0/tasks?project={projectId}&opt_fields=name,notes,permalink_url";

                // Use synchronous GetStringAsync equivalent
                var response = _httpClient.GetStringAsync(url).Result;

                var asanaResponse = JsonConvert.DeserializeObject<AsanaResponse>(response);

                if (asanaResponse?.Data != null)
                {
                    viewModel.Tasks.AddRange(asanaResponse.Data);
                }

                foreach (var item in viewModel.Tasks)
                {
                    var isPresent = timesheetdetails.Any(ts => (ts.Note == item.Name) && (ts.Url.Trim() == item.Permalink_Url));

                    if (isPresent)
                    {
                        matchedViewModel.Tasks.Add(item);
                    }
                }
            }

            string html = GenerateHtmlString(matchedViewModel);
            HtmlTemplate htmlTemplate = new HtmlTemplate();
            if (html != null)
            {
                htmlTemplate.HtmlTemplateName = html;
                htmlTemplate.TimeSheetInputLogId = TimeSheetInputLogID;
                _db.HtmlTemplate.Add(htmlTemplate);
                _db.SaveChanges();
            }
           
        }

        private string GenerateHtmlString(AsanaViewModel viewModel)
        {
            StringBuilder htmlBuilder = new StringBuilder();

            // Define the template with placeholders
            string template = @"
                    <!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Asana Tasks</title>
                        <style>
                            body { font-family: Arial, sans-serif; }
                            ul { list-style-type: none; padding: 0; }
                            li { margin: 10px 0; }
                            h1{ text-align: center;}
                        </style>
                    </head>
                    <body>
                        <h1>$$PROJECHEADING$$</h1>
                        <div>
                            $$TASKTEMPLATE$$    
                        </div>
                    </body>
                    </html>";

            // Generate task items HTML
            StringBuilder tasksBuilder = new StringBuilder();
            int counter = 1;
            foreach (var task in viewModel.Tasks)
            {
                tasksBuilder.Append("<li>");
                tasksBuilder.AppendFormat("{0}. <strong>{1}</strong>", counter, task.Name);
                tasksBuilder.AppendFormat("<p>Link: <a href=\"{0}\">{0}</a></p>", task.Permalink_Url);
                tasksBuilder.AppendFormat("<p>{0}</p>", task.Notes);
                tasksBuilder.Append("</li>");
                counter++;
            }

            // Replace placeholders with actual values
            string htmlString = template
                .Replace("$$PROJECTHEADING$$", "Asana Tasks") // Replace with actual project heading if available
                .Replace("$$TASKTEMPLATE$$", $"<ul>{tasksBuilder.ToString()}</ul>");

            return htmlString;
        }

        //private string GenerateHtmlString(AsanaViewModel viewModel)
        //{
        //    StringBuilder htmlBuilder = new StringBuilder();

        //    htmlBuilder.Append("<!DOCTYPE html>");
        //    htmlBuilder.Append("<html>");
        //    htmlBuilder.Append("<head>");
        //    htmlBuilder.Append("<title>Asana Tasks</title>");
        //    htmlBuilder.Append("<style>");
        //    htmlBuilder.Append("body { font-family: Arial, sans-serif; }");
        //    htmlBuilder.Append("ul { list-style-type: none; padding: 0; }");
        //    htmlBuilder.Append("</style>");
        //    htmlBuilder.Append("</head>");
        //    htmlBuilder.Append("<body>");
        //    htmlBuilder.Append("<h1>Asana Tasks</h1>");
        //    htmlBuilder.Append("<ul>");

        //    int counter = 1;

        //    foreach (var task in viewModel.Tasks)
        //    {
        //        htmlBuilder.Append("<li>");
        //        htmlBuilder.AppendFormat("{0}.<strong>{1}</strong>", counter, task.Name);
        //        htmlBuilder.AppendFormat("<p>Link -> <a href=\"{0}\">{0}</a></p>",task.Permalink_Url);
        //        htmlBuilder.AppendFormat("<p>{0}</p>", task.Notes);
        //        htmlBuilder.Append("</li>");
        //        counter++;
        //    }

        //    htmlBuilder.Append("</ul>");
        //    htmlBuilder.Append("</body>");
        //    htmlBuilder.Append("</html>");

        //    return htmlBuilder.ToString();
        //}
    }
}

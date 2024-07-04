using DocumentFormat.OpenXml.Drawing;
using Entities.Data;
using Entities.Models;
using Entities.ViewModels.TimeSheet;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public TimeSheetRepository(ToDoManagerDBContext toDoManagerDBContext, IConfiguration configuration)
        {
            _db = toDoManagerDBContext;
            _configuration = configuration;

        }

        #region public methods
        /// <summary>
        /// Addtimesheet to the database
        /// </summary>
        /// <param name="timeSheetViewModel">User Id</param>
        /// <returns></returns>
        public async void AddTimeSheetData(TimeSheetViewModel timeSheetViewModel)
        {
            // Check if there's already an active transaction
            try
            {
                // First we added the data of the timesheet input log
                TimeSheetInputLog timeSheetInputLog = new TimeSheetInputLog();
                string[] splitProjectTypeAndId = null;
                timeSheetInputLog.FileName = timeSheetViewModel.UploadedFile.FileName;

                if (timeSheetViewModel.ProjectType == null)
                {
                    AddAll(timeSheetViewModel);
                }
                else
                {
                    using (var transaction = _db.Database.BeginTransaction())
                    {
                        splitProjectTypeAndId = timeSheetViewModel.ProjectType.Split(',');
                        timeSheetInputLog.ProjectType = splitProjectTypeAndId[0];
                        _db.TimeSheetInputLogs.Add(timeSheetInputLog);
                        _db.SaveChanges();


                        ReadAndSaveExcel(timeSheetViewModel, timeSheetInputLog.TimeSheetInputLogID, timeSheetInputLog.ProjectType);

                        var timeSheetDetailData = _db.TimeSheetDetails.Where(s => s.TimeSheetInputLogId == timeSheetInputLog.TimeSheetInputLogID).ToList();

                        if (timeSheetDetailData.Count == 0)
                        {
                            transaction.Rollback();
                            return;
                        }
                        else
                        {
                            transaction.Commit();
                        }

                        if (splitProjectTypeAndId != null && splitProjectTypeAndId.Length > 0)
                        {
                            AsanaTOHtmlDoc(timeSheetInputLog.TimeSheetInputLogID, timeSheetInputLog.ProjectType, splitProjectTypeAndId[1], timeSheetDetailData);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Method for add all the project one by one 
        /// </summary>
        /// <param name="timeSheetViewModel">User Id</param>
        /// <returns></returns>
        public void AddAll(TimeSheetViewModel timeSheetViewModel)
        {
            string allProject = _configuration.GetSection("AllProject").Get<string>();
            string[] splitProject = allProject.Split('|');

            foreach (string line in splitProject)
            {
                TimeSheetViewModel ts = new TimeSheetViewModel();
                ts.UploadedFile = timeSheetViewModel.UploadedFile;
                ts.ProjectType = line;
                AddTimeSheetData(ts);
            }
        }

        /// <summary>
        /// read the excel file and add data in db
        /// </summary>
        /// <param name="timeSheetViewModel">User Id</param>
        /// <param name="timeSheetInputLogId">User Id</param>
        /// <param name="projectType">User Id</param>
        /// <returns></returns>
        public void ReadAndSaveExcel(TimeSheetViewModel timeSheetViewModel, long timeSheetInputLogId, string projectType)
        {
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

                        timeSheetDetail.TimeSheetInputLogId = timeSheetInputLogId;
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
                            if (timeSheetDetail.ProjectType.Contains(projectType))
                            {
                                _db.TimeSheetDetails.Add(timeSheetDetail);
                            }
                        }
                    }
                    else
                    {
                        var timeSheetDetail = new TimeSheetDetails();

                        timeSheetDetail.TimeSheetInputLogId = timeSheetInputLogId;
                        timeSheetDetail.ProjectType = row.Cell(3).GetValue<string>();
                        timeSheetDetail.ProjectCode = row.Cell(4).GetValue<string>();
                        timeSheetDetail.Task = row.Cell(5).GetValue<string>();
                        timeSheetDetail.Note = row.Cell(6).GetValue<string>();
                        timeSheetDetail.Url = row.Cell(15).GetValue<string>();
                        //string date = row.Cell(5).GetValue<string>();
                        //timeSheetDetails. = DateOnly.FromDateTime(DateTime.Parse(date));
                        //Add the Employee to the List of Employees
                        if (timeSheetDetail.ProjectType.Contains(projectType))
                        {
                            _db.TimeSheetDetails.Add(timeSheetDetail);

                        }
                    }
                }

                _db.SaveChanges();
            }
        }

        /// <summary>
        /// Get tasks from calling asana api and match with excel data and get common tasks list
        /// </summary>
        /// <param name="timeSheetInputLogId">User Id</param>
        /// <param name="projectType">User Id</param>
        /// <param name="projectId">User Id</param>
        /// <param name="timesheetdetails">User Id</param>
        /// <returns></returns>
        public void AsanaTOHtmlDoc(long timeSheetInputLogId, string projectType, string projectId, List<TimeSheetDetails> timesheetdetails)
        {
            var viewModel = new AsanaViewModel();
            var matchedViewModel = new AsanaViewModel();

            using (var _httpClient = new HttpClient())
            {
                // Set the Bearer token in the request headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "2/1207683751267451/1207686063568260:d7c2960bcaba823b350bd3531c58d7eb");

                string url = $"https://app.asana.com/api/1.0/tasks?project={projectId}&opt_fields=name,notes,html_notes,permalink_url";

                // Use synchronous GetStringAsync equivalent
                var response = _httpClient.GetStringAsync(url).Result;

                var asanaResponse = JsonConvert.DeserializeObject<AsanaResponse>(response);

                if (asanaResponse?.Data != null)
                {
                    viewModel.Tasks.AddRange(asanaResponse.Data);
                }

                if (timesheetdetails.Count != 0)
                {
                    foreach (var item in viewModel.Tasks)
                    {
                        var isPresent = timesheetdetails.Any(ts => (ts.Note == item.Name) && (ts.Url.Trim() == item.Permalink_Url));

                        if (isPresent)
                        {
                            matchedViewModel.Tasks.Add(item);
                        }
                    }
                }

            }

            string html = matchedViewModel.Tasks.Count == 0 ? null : GenerateHtmlString(matchedViewModel, projectType);
            HtmlTemplate htmlTemplate = new HtmlTemplate();
            if (html != null)
            {
                htmlTemplate.HtmlTemplateName = html;
                htmlTemplate.TimeSheetInputLogId = timeSheetInputLogId;
                _db.HtmlTemplate.Add(htmlTemplate);
                _db.SaveChanges();
            }

        }

        /// <summary>
        /// get document content from db
        /// </summary>
        /// <param name="DocumnentId">User Id</param>
        /// <returns>retutn the html string of the document</returns>
        public string DocumentContent(long documnentId)
        {
            return _db.HtmlTemplate.Where(s => s.HtmlTemplateId == documnentId).Select(s => s.HtmlTemplateName).FirstOrDefault();
        }

        /// <summary>
        /// Getdocument using Id 
        /// </summary>
        /// <param name="documnentId">User Id</param>
        /// <returns>Project document view model</returns>
        public ProjectDocument GetDocumentById(long documnentId)
        {
            var document = _db.HtmlTemplate.Where(s => s.HtmlTemplateId == documnentId).FirstOrDefault();

            ProjectDocument projectDocument = new ProjectDocument();
            if (document != null)
            {
                projectDocument.DocumentId = documnentId;
                projectDocument.HtmlContent = document.HtmlTemplateName;
            }
            return projectDocument;
        }

        /// <summary>
        /// Method for get data of doc for displaying in the index view
        /// </summary>
        /// <param name="">User Id</param>
        /// <returns>timesheetview model</returns>
        public TimeSheetViewModel GetDocumentsData()
        {
            var listOfDoc = _db.HtmlTemplate.ToList();
            TimeSheetViewModel timeSheetViewModel = new TimeSheetViewModel();
            List<ProjectDocument> pd = new List<ProjectDocument>();

            foreach (var item in listOfDoc)
            {
                var data = _db.TimeSheetInputLogs.Where(s => s.TimeSheetInputLogID == item.TimeSheetInputLogId).Select(s => new ProjectDocument
                {
                    DocumentId = item.HtmlTemplateId,
                    ProjectType = s.ProjectType
                });

                pd.AddRange(data);
            }
            timeSheetViewModel.ProjectDocuments = pd;
            return timeSheetViewModel;
        }

        /// <summary>
        /// Method for Updating content
        /// </summary>
        /// <param name="ProjectDocument">User Id</param>
        /// <returns></returns>
        public void UpdateContent(ProjectDocument ProjectDocument)
        {
            var documentInstance = _db.HtmlTemplate.Where(s => s.HtmlTemplateId == ProjectDocument.DocumentId).FirstOrDefault();

            if (documentInstance != null && ProjectDocument.HtmlContent != null)
            {
                documentInstance.HtmlTemplateName = ProjectDocument.HtmlContent;
                _db.HtmlTemplate.Update(documentInstance);
                _db.SaveChanges();
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Get html string for the common task
        /// </summary>
        /// <param name="asanaViewModel">User Id</param>
        /// <param name="projectType">User Id</param>
        /// <returns></returns>
        private string GenerateHtmlString(AsanaViewModel asanaViewModel, string projectType)
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
            </head>
            <body>
                <h1>$$PROJECTHEADING$$ Asana Doc</h1>
                <div>
                    $$TASKTEMPLATE$$    
                </div>
            </body>
            </html>";

            // Generate task items HTML
            StringBuilder tasksBuilder = new StringBuilder();
            int counter = 1;
            foreach (var task in asanaViewModel.Tasks)
            {
                tasksBuilder.Append("<li>");
                tasksBuilder.AppendFormat("<h2><strong>{0}</strong>. <strong>{1}</strong></h2>", counter, task.Name);
                tasksBuilder.AppendFormat("<p>Link: <a href=\"{0}\">{0}</a></p>", task.Permalink_Url);
                tasksBuilder.AppendFormat("<p>{0}</p>", task.Html_Notes);
                tasksBuilder.Append("</li>");
                counter++;
            }

            // Replace placeholders with actual values
            string htmlString = template
                .Replace("$$PROJECTHEADING$$", projectType)
                .Replace("$$TASKTEMPLATE$$", $"<ul>{tasksBuilder.ToString()}</ul>");

            return tasksBuilder.ToString() == null ? null : htmlString;

        }
        #endregion
    }
}

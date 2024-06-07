using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.Data;
using Entities.Models;
using Entities.ViewModels.TimeSheet;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using System.IO;


namespace Repository.Repository
{
    public class TimeSheetRepository : ITimeSheetRepository
    {

        private readonly ToDoManagerDBContext _db;

        public TimeSheetRepository(ToDoManagerDBContext toDoManagerDBContext)
        {
            _db = toDoManagerDBContext;
        }
        public void AddTimeSheetData(TimeSheetViewModel timeSheetViewModel)
        {

            //first we added the data of the timesheet input log
            TimeSheetInputLog timeSheetInputLog = new TimeSheetInputLog();

            if (timeSheetViewModel.FileName == null ) 
            {
                IFormFile file = timeSheetViewModel.UploadedFile;
                timeSheetInputLog.FileName = file.FileName;
            }
            timeSheetInputLog.FileName = timeSheetViewModel.FileName;
            timeSheetInputLog.ProjectType = timeSheetViewModel.ProjectType;

            _db.TimeSheetInputLogs.Add(timeSheetInputLog);
            _db.SaveChanges();

            //now save the data of time sheet details
            using (var workbook = new ClosedXML.Excel.XLWorkbook(timeSheetViewModel.UploadedFile.OpenReadStream()))
            {
                //Lets assume the First Worksheet contains the data
                var worksheet = workbook.Worksheet(1);

                //Lets assume first row contains the header, so skip the first row
                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);

                //Loop Through all the Rows except the first row which contains the header data
                foreach (var row in rows)
                {
                    //get the data from the timesheet deatil table and check whether in the table or not

                    var  list = _db.TimeSheetDetails.ToList();

                    if (list.Count > 0)
                    {
                        var isPresent = list.Any(record => (record.Task == row.Cell(5).GetValue<string>()) && 
                        (record.ProjectCode == row.Cell(4).GetValue<string>()) && (record.Note == row.Cell(6).GetValue<string>()) && 
                        (record.Url == row.Cell(15).GetValue<string>()));

                        if (isPresent) {
                            continue;
                        }
                        else
                        {
                            var timeSheetDetail = new TimeSheetDetails();

                            timeSheetDetail.TimeSheetInputLogId = timeSheetInputLog.TimeSheetInputLogID;
                            timeSheetDetail.ProjectCode = row.Cell(4).GetValue<string>();
                            timeSheetDetail.Task = row.Cell(5).GetValue<string>();
                            timeSheetDetail.Note = row.Cell(6).GetValue<string>();
                            timeSheetDetail.Url = row.Cell(15).GetValue<string>();
                            //string date = row.Cell(5).GetValue<string>();
                            //timeSheetDetails. = DateOnly.FromDateTime(DateTime.Parse(date));

                            //Add the Employee to the List of Employees
                            _db.TimeSheetDetails.Add(timeSheetDetail);
                            _db.SaveChanges();
                        }
                    }
                    else
                    {
                        var timeSheetDetail = new TimeSheetDetails();

                        timeSheetDetail.TimeSheetInputLogId = timeSheetInputLog.TimeSheetInputLogID;
                        timeSheetDetail.ProjectCode = row.Cell(4).GetValue<string>();
                        timeSheetDetail.Task = row.Cell(5).GetValue<string>();
                        timeSheetDetail.Note = row.Cell(6).GetValue<string>();
                        timeSheetDetail.Url = row.Cell(15).GetValue<string>();
                        //string date = row.Cell(5).GetValue<string>();
                        //timeSheetDetails. = DateOnly.FromDateTime(DateTime.Parse(date));

                        //Add the Employee to the List of Employees
                        _db.TimeSheetDetails.Add(timeSheetDetail);
                        _db.SaveChanges();  
                    }
                    
                }

            }   

        }
    }
}

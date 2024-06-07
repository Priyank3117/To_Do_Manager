using BAL;
using Entities.Data;
using Entities.ViewModels.TimeSheet;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class TimeSheetController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly TimeSheetBAL _timeSheetBAL;

        public TimeSheetController( IConfiguration configuration, TimeSheetBAL timeSheetBAL)
        {

            _configuration = configuration; 
            _timeSheetBAL = timeSheetBAL;

        }

        /// <summary>
        /// Main page of the TimeSheet
        /// </summary>
        /// <returns>Timesheet Page</returns>
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            ViewBag.ProjectType = _configuration.GetSection("ProjectTypes").Get<Dictionary<string, string>>();
            return View();
       }

        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(TimeSheetViewModel timeSheetViewModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    //add the data to the database call the method of the   
                    _timeSheetBAL.AddTimeSheetData(timeSheetViewModel);
                  }
               
            }
            catch (Exception)
            {

                throw;
            }
            

            return RedirectToAction("Index"); // Redirect back to upload view in case of failure
        }
    }
}

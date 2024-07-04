using BAL;
using Entities.ViewModels.TimeSheet;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace To_Do_Manager.Controllers
{
    public class TimeSheetController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly TimeSheetBAL _timeSheetBAL;

        public TimeSheetController(IConfiguration configuration, TimeSheetBAL timeSheetBAL)
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
            return View(_timeSheetBAL.GetDocumentsData());
        }


        /// <summary>
        /// import timesheet excel data and processing it
        /// </summary>
        /// <param name="timeSheetViewModel">User Id</param>
        /// <returns></returns>
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

            // Redirect back to upload view in case of failure
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Display texteditor view 
        /// </summary>
        /// <param name="projectDocument">User Id</param>
        /// <returns>view of texteditor</returns>
        public IActionResult TextEditor(ProjectDocument projectDocument)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View(projectDocument);
        }

        /// <summary>
        /// Edit method for opening the document in the texteditor
        /// </summary>
        /// <param name="documnentId">User Id</param>
        /// <returns>view of text editor</returns>
        public IActionResult EditDocument(long documnentId)
        {
            return RedirectToAction("TextEditor", _timeSheetBAL.GetDocumentById(documnentId));
        }

        /// <summary>
        /// Method for save edited content of ProjectDocument
        /// </summary>
        /// <param name="projectDocument">User Id</param>
        /// <returns>r</returns>
        [HttpPost]
        public ActionResult SaveEditDocument(ProjectDocument projectDocument)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _timeSheetBAL.SaveEditDocument(projectDocument);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                return View("TextEditor", projectDocument);
            }
        }

        /// <summary>
        /// Method for downloading the pdf
        /// </summary>
        /// <param name="documnentId">User Id</param>
        /// <returns></returns>
        public IActionResult GeneratePdf(long documnentId)
        { 
            string htmlcontent = _timeSheetBAL.DocumentContent(documnentId); 
            ViewBag.htmlcontent = htmlcontent;
            // Convert the HTML string to a view (needed for Rotativa)
            return new ViewAsPdf("HtmlToPdfProjectDocument", htmlcontent)
            {
                FileName = "Document.pdf",

            };
        }
    }
}

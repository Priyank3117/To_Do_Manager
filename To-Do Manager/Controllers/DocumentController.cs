using BAL;
using Entities.ViewModels.DocumentViewModels;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace To_Do_Manager.Controllers
{
    public class DocumentController : Controller
    {

        private readonly DocumentBAL _documentBAL;
        private readonly IWebHostEnvironment _env;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public DocumentController(DocumentBAL documentBAL,IWebHostEnvironment webHostEnvironment) 
        { 
            _documentBAL = documentBAL;
            _webHostEnvironment = webHostEnvironment;
            
        }
        /// <summary>
        /// Main page of the Documents
        /// </summary>
        /// <returns>Documents Page</returns>
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View(_documentBAL.GetAllAvailableDocs(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }


        /// <summary>
        /// Get Available All Teams with Team Name
        /// </summary>
        /// <param name="searchTerm">Search Keyword</param>
        /// <returns>List of all available teams</returns>
        //public List<AllTeamsViewModel> GetAllAvailableTeams()
        //{
        //    return;
        //}


        /// <summary>
        /// Get Available All Docs Corresponding that team
        /// </summary>
        /// <param name="teamId,teamName">Search Keyword</param>
        /// <returns>List of all available teams</returns>
        //public IActionResult DocumentPage(long teamId,string teamName)
        //{
        //    ViewBag.UserName = HttpContext.Session.GetString("UserName");
        //    ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
        //    ViewBag.TeamId = teamId;
        //    ViewBag.TeamName = teamName; 
        //    return View(_documentBAL.GetDocuments(teamId));
        //}


        /// <summary>
        /// get the view of the text editor
        /// </summary>
        /// <param name="TeamId">Search Keyword</param>
        /// <returns>view of the text editor </returns>
        public IActionResult TextEditor(long TeamId, string teamName)
        {
            long l;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            DocumentViewModel documentViewModel = new DocumentViewModel();
            documentViewModel.TeamId = TeamId;
            documentViewModel.TeamName = teamName;
            Int64.TryParse(HttpContext.Session.GetString("UserId"), out l);
            documentViewModel.UserId = l;
            return View(documentViewModel);
        }

        /// <summary>
        /// upload the image on the server
        /// </summary>
        /// <param name="file"></param>
        /// <returns>url of the image</returns>
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded" });
            }

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = DateTime.Now.Ticks + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var fileUrl = $"/uploads/{fileName}";
            return Ok(new { url = fileUrl });
        }


        /// <summary>
        /// Add the data of the text editor to the database
        /// </summary>
        /// <param name="documentViewModel">Search Keyword</param>
        /// <returns>Redirect to the DocumentPage </returns>
        [HttpPost]
        public ActionResult SaveContent(DocumentViewModel documentViewModel)
        {
            if(ModelState.IsValid)
            {
                try
                {
                   _documentBAL.SaveContent(documentViewModel);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                  throw new Exception(ex.ToString());
                }
            }
            else
            {
                return View("TextEditor", documentViewModel);
            }

        }

        /// <summary>
        /// open document in the editor for the edit the data
        /// </summary>
        /// <param name="DocumnentId">Search Keyword</param>
        /// <returns>edit view of the document</returns>
        public IActionResult EditDocument(long DocumnentId,string teamName)
        {
            return View("TextEditor", _documentBAL.GetDocumentById(DocumnentId,teamName));
        }


        /// <summary>
        /// download the pdf form
        /// </summary>
        /// <param name="DocumnentId">Search Keyword</param>
        /// <returns>download the pdf</returns>
        public IActionResult GeneratePdf(long DocumnentId,string flag)
        {

            //flag 1 for html
            //flag 2 for pdf

            string htmlcontent = _documentBAL.DocumentContent(DocumnentId);
            ViewBag.flag = flag;
            ViewBag.htmlcontent = htmlcontent;
            // Convert the HTML string to a view (needed for Rotativa)
            return new ViewAsPdf("HtmlToPdfDocument", htmlcontent)
            {
                FileName = "Document.pdf",
                
            };
        }


        
    }
}

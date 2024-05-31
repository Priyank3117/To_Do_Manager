using BAL;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class DocumentController : Controller
    {

        private readonly DocumentBAL _documentBAL;

        public DocumentController(DocumentBAL documentBAL) 
        { 
            _documentBAL = documentBAL;
        }
        /// <summary>
        /// Main page of the Documents
        /// </summary>
        /// <returns>Documents Page</returns>
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View();
        }


        /// <summary>
        /// Get Available All Teams with Team Name
        /// </summary>
        /// <param name="searchTerm">Search Keyword</param>
        /// <returns>List of all available teams</returns>
        public List<AllTeamsViewModel> GetAllAvailableTeams(string searchTerm)
        {
            return _documentBAL.GetAllAvailableTeams(searchTerm, long.Parse(HttpContext.Session.GetString("UserId")!));
        }


        /// <summary>
        /// Get Available All Docs Corresponding that team
        /// </summary>
        /// <param name="teamId">Search Keyword</param>
        /// <returns>List of all available teams</returns>
        public IActionResult DocumentPage(long teamId)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View();
        }

        public IActionResult TextEditor()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View();
        }
    }
}

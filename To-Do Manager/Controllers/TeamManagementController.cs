using BAL;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class TeamManagementController : Controller
    {
        private TeamManagementBAL _TeamManagementBAL;
        private HomeBAL _HomeBAL;

        public TeamManagementController(TeamManagementBAL teamManagementBAL, HomeBAL homeBAL)
        {
            _TeamManagementBAL = teamManagementBAL;
            _HomeBAL = homeBAL;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

            return View();
        }

        public IActionResult GetAllTeams()
        {
            return PartialView("~/Views/PartialViews/TeamManagement/_AllTeamsForJoinTeam.cshtml", _HomeBAL.GetAllTeams("", long.Parse(HttpContext.Session.GetString("UserId")!)));
        }
        
        public IActionResult GetAllTeamDetailsPartialView()
        {
            return PartialView("~/Views/PartialViews/TeamManagement/_AllTeamWithDetails.cshtml", _TeamManagementBAL.GetAllTeamsDetails(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }
    }
}

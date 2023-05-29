using BAL;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using To_Do_Manager.Models;

namespace To_Do_Manager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeBAL _HomeBAL;

        public HomeController(ILogger<HomeController> logger, HomeBAL homeBAL)
        {
            _logger = logger;
            _HomeBAL = homeBAL;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View();
        }

        public IActionResult AllTeamsPage()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View(_HomeBAL.GetAllTodayTasks(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }
        
        public List<AllTeamsViewModel> GetAllTeams(string searchTerm)
        {
            return _HomeBAL.GetAllTeams(searchTerm, long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        public TeamViewModel GetTeamDetails(long teamId)
        {
            return _HomeBAL.GetTeamDetails(teamId);
        }

        public bool CreateTeam(TeamViewModel team)
        {
            if (ModelState.IsValid)
            {
                team.TeamLeaderUserId = long.Parse(HttpContext.Session.GetString("UserId")!);
                return _HomeBAL.CreateTeam(team);
            }
            return false;
        }

        public bool RequestToJoinTeam(TeamMemberViewModel userRequest)
        {
            userRequest.UserId = long.Parse(HttpContext.Session.GetString("UserId")!);
            return _HomeBAL.RequestToJoinTeam(userRequest);
        }

        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            return _HomeBAL.GetDataForAddTask(teamId, userId);
        }

        public bool AddTask(TaskDetailViewModel task)
        {
            if(task.UserId == 0)
            {
                task.UserId = long.Parse(HttpContext.Session.GetString("UserId")!);
            }
            return _HomeBAL.AddTask(task);
        }

        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel task)
        {
            return _HomeBAL.MarkTaskAsCompleteOrUncomplete(task);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
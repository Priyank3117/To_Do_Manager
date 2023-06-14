using BAL;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using To_Do_Manager.Filters;
using To_Do_Manager.Models;

namespace To_Do_Manager.Controllers
{
    [CheckSessionFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HomeBAL _HomeBAL;
        private readonly AccountBAL _AccountBAL;

        public HomeController(ILogger<HomeController> logger, HomeBAL homeBAL, AccountBAL accountBAL)
        {
            _logger = logger;
            _HomeBAL = homeBAL;
            _AccountBAL = accountBAL;
        }

        /// <summary>
        /// Get Available All Teams Page
        /// </summary>
        /// <returns>Availale Teams Page</returns>
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

            return View();
        }

        /// <summary>
        /// Go to Home Page
        /// </summary>
        /// <returns>Return All Teams Page if user have any team else return All Available Team Page</returns>
        public IActionResult GoToHome()
        {
            if (!_AccountBAL.IsUserHaveAnyTeam(long.Parse(HttpContext.Session.GetString("UserId")!)))
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("AllTeamsPage", "Home");
        }

        /// <summary>
        /// Get All Teams of Users
        /// </summary>
        /// <returns>All Teams with Today's Task</returns>
        public IActionResult AllTeamsPage()
        {

            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");
            return View(_HomeBAL.GetAllTodayTasks(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        /// <summary>
        /// Get Available All Teams with Team Name
        /// </summary>
        /// <param name="searchTerm">Search Keyword</param>
        /// <returns>List of all available teams</returns>
        public List<AllTeamsViewModel> GetAllTeams(string searchTerm)
        {
            return _HomeBAL.GetAllTeams(searchTerm, long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        /// <summary>
        /// Get Teams Details
        /// </summary>
        /// <param name="teamId">TeamId</param>
        /// <returns>All Tasks with Team Name</returns>
        public TeamViewModel GetTeamDetails(long teamId)
        {
            return _HomeBAL.GetTeamDetails(teamId);
        }

        /// <summary>
        /// Create Team
        /// </summary>
        /// <param name="team">Team Details like Team Name, Team Discription and Team Members</param>
        /// <returns>True - If successfully team created else False</returns>
        public bool CreateTeam(TeamViewModel team)
        {
            if (ModelState.IsValid)
            {
                team.TeamLeaderUserId = long.Parse(HttpContext.Session.GetString("UserId")!);
                return _HomeBAL.CreateTeam(team);
            }
            return false;
        }

        /// <summary>
        /// Request to Join Team
        /// </summary>
        /// <param name="userRequest">UserId, TeamId</param>
        /// <returns>True - If successfully requested alse False</returns>
        public bool RequestToJoinTeam(TeamMemberViewModel userRequest)
        {
            userRequest.UserId = long.Parse(HttpContext.Session.GetString("UserId")!);
            return _HomeBAL.RequestToJoinTeam(userRequest);
        }

        #region AddTask
        /// <summary>
        /// Get Data for Add Task
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>List of User's UserName with User Id</returns>
        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            return _HomeBAL.GetDataForAddTask(teamId, userId);
        }

        public List<ListOfUsers> GetDataForAddTaskToTeamMember(long teamId)
        {
            return _HomeBAL.GetDataForAddTask(teamId, long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        /// <summary>
        /// Add Task In Team
        /// </summary>
        /// <param name="task">Task Name, Team Id and User Id</param>
        /// <returns>True - If successfully Add Task alse False</returns>
        public bool AddTask(TaskDetailViewModel task)
        {
            if (task.UserId == 0)
            {
                task.UserId = long.Parse(HttpContext.Session.GetString("UserId")!);
            }
            task.FromUserId = long.Parse(HttpContext.Session.GetString("UserId")!);

            return _HomeBAL.AddTask(task);
        }
        #endregion

        /// <summary>
        /// Mark Task as Complete Or Uncomplete
        /// </summary>
        /// <param name="task">Task Id, Team Id and User Id</param>
        /// <returns>True - If successful alse False</returns>
        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel task)
        {
            return _HomeBAL.MarkTaskAsCompleteOrUncomplete(task);
        }

        /// <summary>
        /// Get Task Details
        /// </summary>
        /// <param name="taskId">Task Id</param>
        /// <returns>Task Details like - Task Name, Task Description, Start Date, End Date and IsCompleted or Not</returns>
        public TaskDetailViewModel GetTaskDetails(long taskId)
        {
            return _HomeBAL.GetTaskDetails(taskId);
        }

        /// <summary>
        /// Get All Notifications
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List od all notifications</returns>
        public IActionResult GetNotifications()
        {
            return PartialView("~/Views/PartialViews/_Notification.cshtml", _HomeBAL.GetNotifications(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        /// <summary>
        /// Clear All Notification
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If all notifications cleared else False</returns>
        public bool ClearAllNotifications()
        {
            return _HomeBAL.ClearAllNotifications(long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        public bool MarkNotificationAsRead(long notificationId)
        {
            return _HomeBAL.MarkNotificationAsRead(notificationId);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
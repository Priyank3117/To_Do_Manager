using BAL;
using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using To_Do_Manager.Filters;

namespace To_Do_Manager.Controllers
{
    [CheckSessionFilter]
    public class AllTasksController : Controller
    {
        private readonly AllTasksBAL _AllTaksBAL;

        public AllTasksController(AllTasksBAL allTaksBAL)
        {
            _AllTaksBAL = allTaksBAL;
        }

        /// <summary>
        /// Get All Tasks Page
        /// </summary>
        /// <returns>All Tasks Page With Calender and List View</returns>
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

            return View(_AllTaksBAL.GetTasksForCalenderView(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        /// <summary>
        /// Get Data For Add Task To Today Task
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>List of Users details like Avatar, FirstName, LastName, and User Id</returns>
        public List<ListOfUsers> GetForAddTaskToToDo(long teamId)
        {
            return _AllTaksBAL.GetForAddTaskToToDo(teamId, long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        /// <summary>
        /// Add Task To To-Do Page
        /// </summary>
        /// <param name="task">Teak Detail View Model for storing Task Id, Team Id and User Id</param>
        /// <returns>True - If Successfully Added alse False</returns>
        public bool AddTaskToTodayTask(TaskDetailViewModel task)
        {
            return _AllTaksBAL.AddTaskToTodayTask(task);
        }

        /// <summary>
        /// Add Task To Today Task For TeamMember
        /// </summary>
        /// <param name="task">TaskDetails like User Id of that task and User Id of who task assigned</param>
        /// <returns>True - If task successfully added to Today task else False</returns>
        public bool AddTaskToTodayTaskForTeamMember(TaskDetailViewModel task)
        {
            task.FromUserId = long.Parse(HttpContext.Session.GetString("UserId")!);

            return _AllTaksBAL.AddTaskToTodayTaskForTeamMember(task);
        }

        /// <summary>
        /// Get All Task In All Task Page Team Wise
        /// </summary>
        /// <param name="filter">filter params like Team Name, Task Name, Start Date, End Date and Task Status</param>
        /// <returns>List of All Task Team wise</returns>
        public IActionResult GetAllTaskOfAllTeams(Filter filter)
        {
            filter.UserId = long.Parse(HttpContext.Session.GetString("UserId")!);

            return PartialView("~/Views/PartialViews/AllTasks/_Team.cshtml", _AllTaksBAL.GetAllTasks(filter));
        }
    }
}

using BAL;
using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
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
        /// Add Task To To-Do Page
        /// </summary>
        /// <param name="task">Teak Detail View Model for storing Task name</param>
        /// <returns>True - If Successfully Added alse False</returns>
        public bool AddTaskToTodayTask(TaskDetailViewModel task)
        {
            return _AllTaksBAL.AddTaskToTodayTask(task);
        }

        /// <summary>
        /// Get All Tasks for List View
        /// </summary>
        /// <param name="searchTerm">Search Keyword</param>
        /// <returns>Partial Views Team wise with Tasks data</returns>
        public IActionResult GetAllTaskOfAllTeams(Filter filter)
        {
            filter.UserId = long.Parse(HttpContext.Session.GetString("UserId")!);

            return PartialView("~/Views/PartialViews/AllTasks/_Team.cshtml", _AllTaksBAL.GetAllTasks(filter));
        }
    }
}

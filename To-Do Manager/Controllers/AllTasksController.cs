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

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

            return View(_AllTaksBAL.GetTasksForCalenderView(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        public bool AddTaskToTodayTask(TaskDetailViewModel task)
        {
            return _AllTaksBAL.AddTaskToTodayTask(task);
        }

        public IActionResult GetAllTaskOfAllTeams(string searchTerm)
        {
            return PartialView("~/Views/PartialViews/AllTasks/_Team.cshtml", _AllTaksBAL.GetAllTasks(long.Parse(HttpContext.Session.GetString("UserId")!), searchTerm));
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class AllTasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

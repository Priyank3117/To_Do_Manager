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
            return View();
        }

        public IActionResult AllTeamsPage()
        {
            return View();
        }

        public bool CreateTeam(TeamViewModel team)
        {
            if (ModelState.IsValid)
            {
                return _HomeBAL.CreateTeam(team);
            }
            return false;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
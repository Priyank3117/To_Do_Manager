using BAL;
using Entities.ViewModels.UserProfileViewModels;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserProfileBAL _UserProfileBAL;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserProfileController(UserProfileBAL userProfileBAL, IWebHostEnvironment webHostEnvironment)
        {
            _UserProfileBAL = userProfileBAL;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

            return View(_UserProfileBAL.GetUserProfileDetails(long.Parse(HttpContext.Session.GetString("UserId")!)));
        }

        [HttpPost]
        public IActionResult Index(UserProfileViewModel userProfile)
        {
            if (ModelState.IsValid)
            {
                if (_UserProfileBAL.SaveUserProfileDetails(userProfile))
                {
                    return View();
                }
            }
            return View();
        }

        public string ChangePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == newPassword)
            {
                return "SamePassword";
            }
            else if (newPassword == null)
            {
                return "New Password Required";
            }
            else if (oldPassword == null)
            {
                return "Old Password Required";
            }
            else if (newPassword.Length < 8)
            {
                return "Minumum length is 8 Character";
            }

            return _UserProfileBAL.ChangePassword(long.Parse(HttpContext.Session.GetString("UserId")!), newPassword, oldPassword);
        }

        public string ChangeImage(IFormFile file)
        {
            var ImageURL = UploadImage(file);

            HttpContext.Session.SetString("Avatar", ImageURL);

            return _UserProfileBAL.ChangeImage(long.Parse(HttpContext.Session.GetString("UserId")!), ImageURL);
        }

        private string UploadImage(IFormFile file)
        {
            string folder = "ProfileImages/";
            folder += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            file.CopyTo(new FileStream(serverFolder, FileMode.Create));

            return "/" + folder;
        }

        public List<ListOfTeamsName> GetTeamNames()
        {
            return _UserProfileBAL.GetTeamNames(long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        public bool LeaveFromTeam(long teamId, long userId)
        {
            return _UserProfileBAL.LeaveFromTeam(teamId, userId);
        }

        public bool LeaveFromAllTeam()
        {
            return _UserProfileBAL.LeaveFromAllTeam(long.Parse(HttpContext.Session.GetString("UserId")!));
        }
    }
}

using BAL;
using Entities.ViewModels.UserProfileViewModels;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserProfileBAL _UserProfileBAL;

        public UserProfileController(UserProfileBAL userProfileBAL)
        {
            _UserProfileBAL = userProfileBAL;
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

        public string ChangeImage(string imageURL)
        {
            return _UserProfileBAL.ChangeImage(long.Parse(HttpContext.Session.GetString("UserId")!), imageURL);
        }
    }
}

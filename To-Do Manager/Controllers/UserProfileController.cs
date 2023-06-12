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

        /// <summary>
        /// Get User Profile Page
        /// </summary>
        /// <returns>User Profile Page</returns>
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            else
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
                ViewBag.Avatar = HttpContext.Session.GetString("Avatar");

                return View(_UserProfileBAL.GetUserProfileDetails(long.Parse(HttpContext.Session.GetString("UserId")!)));
            }
        }

        /// <summary>
        /// Save User Profile's Data
        /// </summary>
        /// <param name="userProfile">User's Details like FirstName, LastName, Gender, Department and LinkedInURL</param>
        /// <returns>User Profile Page if any validation error then with error</returns>
        [HttpPost]
        public IActionResult Index(UserProfileViewModel userProfile)
        {
            if (ModelState.IsValid)
            {
                var userDetails = _UserProfileBAL.SaveUserProfileDetails(userProfile);
                ViewBag.Avatar = userDetails.Avatar;
                ViewBag.UserName = userDetails.FirstName + " " + userDetails.LastName;
                HttpContext.Session.SetString("UserName", userDetails.FirstName + " " + userDetails.LastName);

                return View(userDetails);
            }
            return View();
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="oldPassword">Old Password of User</param>
        /// <param name="newPassword">New Password of User</param>
        /// <returns>String with operation status</returns>
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

        #region ChangeAvatar
        /// <summary>
        /// Change User's Avatar
        /// </summary>
        /// <param name="file">Image file that selected by user</param>
        /// <returns>"Changed" if successfully changed</returns>
        public string ChangeImage(IFormFile file)
        {
            var ImageURL = UploadImage(file);

            HttpContext.Session.SetString("Avatar", ImageURL);

            return _UserProfileBAL.ChangeImage(long.Parse(HttpContext.Session.GetString("UserId")!), ImageURL);
        }

        /// <summary>
        /// Upload Avatar in Local Folder
        /// </summary>
        /// <param name="file">Image File</param>
        /// <returns>Stored path of that Image file</returns>
        private string UploadImage(IFormFile file)
        {
            string folder = "ProfileImages/";
            folder += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
            file.CopyTo(new FileStream(serverFolder, FileMode.Create));

            return "/" + folder;
        }
        #endregion

        #region LeaveFromTeam
        /// <summary>
        /// Get All Teams Name for Leave Team
        /// </summary>
        /// <returns>List of all team names</returns>
        public List<ListOfTeamsName> GetTeamNames()
        {
            return _UserProfileBAL.GetTeamNames(long.Parse(HttpContext.Session.GetString("UserId")!));
        }

        /// <summary>
        /// Leave From Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>True - If successfully leaved from team else False</returns>
        public bool LeaveFromTeam(long teamId, long userId)
        {
            return _UserProfileBAL.LeaveFromTeam(teamId, userId);
        }

        /// <summary>
        /// Leave from All Team
        /// </summary>
        /// <returns>True - If successfully leaved from all teams else False</returns>
        public bool LeaveFromAllTeam()
        {
            return _UserProfileBAL.LeaveFromAllTeam(long.Parse(HttpContext.Session.GetString("UserId")!));
        }
        #endregion
    }
}

using BAL;
using Entities;
using Entities.ViewModels.AccountViewModels;
using Entities.ViewModels.HomeViewModels;
using Microsoft.AspNetCore.Mvc;

namespace To_Do_Manager.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountBAL _AccountBAL;
        private readonly MailHelper _MailHelper;

        public AccountController(AccountBAL accountBAL, MailHelper mail)
        {
            _AccountBAL = accountBAL;
            _MailHelper = mail;
        }

        #region Login
        /// <summary>
        /// Very first page of To-Do Manager Project
        /// </summary>
        /// <returns>Login Page</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Post method of Login
        /// </summary>
        /// <param name="login">Login View Model for storing user's crediantials</param>
        /// <returns>If User already have a team then - AllTeamsPage otherwise Index page of Home controller</returns>
        [HttpPost]
        public IActionResult Index(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var user = _AccountBAL.UserLogin(login.Email);
                if (user.Email == "")
                {
                    ModelState.AddModelError("Email", "User not exist!");
                }
                else
                {
                    if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                    {
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());
                        HttpContext.Session.SetString("UserName", user.UserName);
                        HttpContext.Session.SetString("Avatar", user.Avatar);

                        if (!_AccountBAL.IsUserHaveAnyTeam(user.UserId))
                            return RedirectToAction("Index", "Home");
                        else
                            return RedirectToAction("AllTeamsPage", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect Password");
                    }
                }
            }
            return View(login);
        }
        #endregion

        #region Registration
        /// <summary>
        /// Get Register User Page
        /// </summary>
        /// <returns>Register User Page</returns>
        public IActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Post Mothod for Register User
        /// </summary>
        /// <param name="registration">Registration View Model for storing User's details</param>
        /// <returns>If Model State is valid then - Login Page</returns>
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                if (_AccountBAL.IsUserAlreadyRegistered(registration.Email))
                {
                    ModelState.AddModelError("Email", "User already registered!");
                }
                else
                {
                    registration.Password = BCrypt.Net.BCrypt.HashPassword(registration.Password);
                    if (_AccountBAL.RegisterUser(registration))
                    {
                        return RedirectToAction("Index", "Account");
                    }
                }
            }
            return View(registration);
        }
        #endregion

        #region ForgotPassword
        /// <summary>
        /// Get Forgot Password Page
        /// </summary>
        /// <returns>Forgot Password Page</returns>
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Send OTP via email to user
        /// </summary>
        /// <param name="forgotPassword">Forgot Password View Model for storing user's email</param>
        /// <returns>True - if OTP successfully send alse False</returns>
        public bool SendOTP(ForgotPasswordViewModel forgotPassword)
        {
            if (_AccountBAL.IsUserAlreadyRegistered(forgotPassword.Email))
            {
                Random otp = new Random();
                var OTP = otp.Next(0000, 9999);
                forgotPassword.OTP = OTP;

                SendEmailViewModel sendEmailViewModel = new()
                {
                    Body = "Your OTP is " + OTP,
                    Subject = "OTP for Reset Password",
                    ToEmail = forgotPassword.Email,
                };
                
                if (_AccountBAL.StoreOTP(forgotPassword) && _MailHelper.SendEmail(sendEmailViewModel))
                    return true;
            }
            return false;
        }

        public string VerifyOTP(ForgotPasswordViewModel forgotPassword)
        {
            return _AccountBAL.VerifyOTP(forgotPassword);
        }

        /// <summary>
        /// Get Change Password Partial View
        /// </summary>
        /// <returns>Change Password Partial View</returns>
        public IActionResult GetChangePasswordView()
        {
            return PartialView("~/Views/Account/ResetPassword.cshtml");
        }

        /// <summary>
        /// Get Forgot Password Partial View
        /// </summary>
        /// <returns>Forgot Password Partial View</returns>
        public IActionResult GetForgotPasswordView()
        {
            return PartialView("~/Views/Account/ForgotPassword.cshtml");
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="resetPassword">Reset Password View Model for storing Old Password and New Password</param>
        /// <returns> "Changed" - If Old password is valid alse "InvalidEmail"</returns>
        public string ChangePassword(ResetPasswordViewModel resetPassword)
        {
            return _AccountBAL.ChangePassword(resetPassword);
        }
        #endregion

        #region Logout
        /// <summary>
        /// Logout
        /// </summary>
        /// <returns>Login Page</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Avatar");
            HttpContext.Session.Remove("UserId");

            return RedirectToAction("Index", "Account");
        }
        #endregion
    }
}
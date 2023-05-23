using Entities.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using System.Net;
using System.Net.Mail;

namespace To_Do_Manager.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _AccountRepo;

        public AccountController(IAccountRepository accountRepo)
        {
            _AccountRepo = accountRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var user = _AccountRepo.UserLogin(login.Email);
                if (user.Email == "")
                {
                    ModelState.AddModelError("Email", "User not exist!");
                }
                else
                {
                    if (BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                    {
                        HttpContext.Session.SetString("UserId", user.UserId.ToString());

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect Password");
                    }
                }
            }
            return View(login);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel registration)
        {
            if (ModelState.IsValid)
            {
                if (_AccountRepo.IsUserAlreadyRegistered(registration.Email))
                {
                    ModelState.AddModelError("Email", "User already registered!");
                }
                else if (!registration.Password.Contains("@"))
                {
                    ModelState.AddModelError("Password", "Enter strong password");
                }
                else
                {
                    registration.Password = BCrypt.Net.BCrypt.HashPassword(registration.Password);
                    if (_AccountRepo.RegisterUser(registration))
                    {
                        return RedirectToAction("Index", "Account");
                    }
                }
            }
            return View(registration);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (ModelState.IsValid)
            {
                if (!_AccountRepo.IsUserAlreadyRegistered(forgotPassword.Email))
                {
                    ModelState.AddModelError("Email", "User not exist!");
                }
                else
                {

                }
            }
            return View();
        }

        public bool SendOTP(ForgotPasswordViewModel forgotPassword)
        {
            if (_AccountRepo.IsUserAlreadyRegistered(forgotPassword.Email))
            {
                var fromEmail = new MailAddress("chavdaanand2002@gmail.com");
                Random otp = new Random();
                var OTP = otp.Next(0000, 9999);

                MailMessage message = new MailMessage(fromEmail, new MailAddress(forgotPassword.Email))
                {
                    Subject = "OTP for Reset Password",
                    Body = "Your OTP is " + OTP,
                    IsBodyHtml = true
                };

                new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, "aaafknrnkglrohrn")
                }.Send(message);

                forgotPassword.OTP = OTP;
                if (_AccountRepo.StoreOTP(forgotPassword))
                    return true;
            }
            return false;
        }

        public string VerifyOTP(ForgotPasswordViewModel forgotPassword)
        {
            return _AccountRepo.VerifyOTP(forgotPassword);
        }
        
        public IActionResult GetChangePasswordView()
        {
            return PartialView("~/Views/Account/ResetPassword.cshtml");
        }
        
        public IActionResult GetForgotPasswordView()
        {
            return PartialView("~/Views/Account/ForgotPassword.cshtml");
        }
    }
}

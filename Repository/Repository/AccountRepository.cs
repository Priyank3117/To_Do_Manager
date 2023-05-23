using Entities.Data;
using Entities.Models;
using Entities.ViewModels.AccountViewModels;
using Repository.Interface;

namespace Repository.Repository
{
    public class AccountRepository: IAccountRepository
    {
        private readonly ToDoManagerDBContext _db;

        public AccountRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        public bool RegisterUser(RegistrationViewModel registration)
        {
            if(registration != null)
            {
                Users user = new Users()
                {
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    Email = registration.Email,
                    Password = registration.Password,
                    Role = registration.Role,
                    CreatedAt = DateTime.Now
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                return true;
            }
            return false;
        }

        public bool IsUserAlreadyRegistered(string email)
        {
            return _db.Users.Any(user => user.Email == email);
        }

        public LoginViewModel UserLogin(string email)
        {
            LoginViewModel loginViewModel = new LoginViewModel();
            var user = _db.Users.Where(user => user.Email == email).FirstOrDefault();

            if (user != null)
            {
                loginViewModel.UserId = user.UserId;
                loginViewModel.Email = user.Email;
                loginViewModel.Password = user.Password;
            }
            return loginViewModel;
        }

        public bool StoreOTP(ForgotPasswordViewModel forgotPassword)
        {
            if (forgotPassword.Email != "")
            {
                var resetPasswordEntry = _db.ResetPassword.FirstOrDefault(resetPassword => resetPassword.Email == forgotPassword.Email);
                if (resetPasswordEntry == null)
                {
                    ResetPassword resetPassword = new ResetPassword()
                    {
                        Email = forgotPassword.Email,
                        OTP = forgotPassword.OTP,
                        CreatedAt = DateTime.Now
                    };

                    _db.Add(resetPassword);
                    _db.SaveChanges();
                }
                else
                {
                    resetPasswordEntry.OTP = forgotPassword.OTP;
                    resetPasswordEntry.Email = forgotPassword.Email;
                    resetPasswordEntry.UpdatedAt = DateTime.Now;

                    _db.Update(resetPasswordEntry);
                    _db.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public string VerifyOTP(ForgotPasswordViewModel forgotPassword)
        {
            var otpDetails = _db.ResetPassword.FirstOrDefault( resetPassword => resetPassword.Email == forgotPassword.Email);
            if(otpDetails == null)
            {
                return "FirstGenerateOTP";
            }
            else
            {
                var OTPTime = otpDetails.UpdatedAt == null ? otpDetails.CreatedAt : otpDetails.UpdatedAt;
                if ( OTPTime < DateTime.Now.AddMinutes(-10))
                {
                    return "OTPIsExpired";
                }
                else if( otpDetails.OTP == forgotPassword.OTP)
                {
                    return "ValidOTP";
                }

                return "Error";
            }
        }
    }
}

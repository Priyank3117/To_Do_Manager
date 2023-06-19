using Entities.Data;
using Entities.Models;
using Entities.ViewModels.AccountViewModels;
using Repository.Interface;

namespace Repository.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ToDoManagerDBContext _db;
        private readonly IHomeRepository _HomeRepo;

        public AccountRepository(ToDoManagerDBContext db, IHomeRepository homeRepo)
        {
            _db = db;
            _HomeRepo = homeRepo;
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registration">User's details like first name, last name, email, password</param>
        /// <returns>True - If successfully register else False</returns>
        public bool RegisterUser(RegistrationViewModel registration)
        {
            if (registration != null)
            {
                Users user = new Users()
                {
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    Email = registration.Email,
                    Password = registration.Password,
                    CreatedAt = DateTime.Now
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                var isInvitedUser = _db.InvitedUsers.FirstOrDefault(user => user.Email == registration.Email);
                if (isInvitedUser != null)
                {
                    _HomeRepo.AddUserToTeam(isInvitedUser.Email, isInvitedUser.TeamId);

                    _db.Remove(isInvitedUser);
                    _db.SaveChanges();
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check User Already Register or Not
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns>True - If user already register else False</returns>
        public bool IsUserAlreadyRegistered(string email)
        {
            return _db.Users.Any(user => user.Email == email);
        }

        /// <summary>
        /// Ligin Method for User
        /// </summary>
        /// <param name="email">Email of User</param>
        /// <returns>User's details like email, password, user Id</returns>
        public LoginViewModel UserLogin(string email)
        {
            LoginViewModel loginViewModel = new();
            var user = _db.Users.Where(user => user.Email == email).FirstOrDefault();

            if (user != null)
            {
                loginViewModel.UserId = user.UserId;
                loginViewModel.Email = user.Email;
                loginViewModel.Password = user.Password;
                loginViewModel.UserName = user.FirstName + " " + user.LastName;
                loginViewModel.Avatar = user.Avatar;
            }
            return loginViewModel;
        }

        /// <summary>
        /// Store OTP
        /// </summary>
        /// <param name="forgotPassword">OTP and user's email Id</param>
        /// <returns>True - If OTP successfully stored else False</returns>
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

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="forgotPassword">User's Email and their OTP</param>
        /// <returns>"Valid OTP" - If OTP is valid</returns>
        public string VerifyOTP(ForgotPasswordViewModel forgotPassword)
        {
            var otpDetails = _db.ResetPassword.FirstOrDefault(resetPassword => resetPassword.Email == forgotPassword.Email);
            if (otpDetails == null)
            {
                return "FirstGenerateOTP";
            }
            else
            {
                var OTPTime = otpDetails.UpdatedAt == null ? otpDetails.CreatedAt : otpDetails.UpdatedAt;
                if (OTPTime < DateTime.Now.AddMinutes(-10))
                {
                    return "OTPIsExpired";
                }
                else if (otpDetails.OTP == forgotPassword.OTP)
                {
                    return "ValidOTP";
                }
                else if (otpDetails.OTP != forgotPassword.OTP)
                {
                    return "InvalidOTP";
                }

                return "Error";
            }
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="resetPassword">old password, new password, email of user</param>
        /// <returns>"Changed" - If password changed</returns>
        public string ChangePassword(ResetPasswordViewModel resetPassword)
        {
            var user = _db.Users.FirstOrDefault(user => user.Email == resetPassword.Email);

            resetPassword.NewPassword = BCrypt.Net.BCrypt.HashPassword(resetPassword.NewPassword);

            if (user != null)
            {
                user.Password = resetPassword.NewPassword;
                user.UpdatedAt = DateTime.Now;

                _db.Update(user);
                _db.SaveChanges();

                return "Changed";
            }
            else
            {
                return "EnterValidOldPassword";
            }
        }

        /// <summary>
        /// Check If User Have Any Team or Not
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns>True - If User have team else False</returns>
        public bool IsUserHaveAnyTeam(long userID)
        {
            return _db.TeamMembers.Any(teamMember => teamMember.UserId == userID && teamMember.Status == TeamMembers.MemberStatus.Approved);
        }
    }
}

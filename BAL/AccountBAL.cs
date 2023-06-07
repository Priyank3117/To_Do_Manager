using Entities.ViewModels.AccountViewModels;
using Repository.Interface;

namespace BAL
{
    public class AccountBAL
    {
        private readonly IAccountRepository _AccountRepo;

        public AccountBAL(IAccountRepository accountRepo)
        {
            _AccountRepo = accountRepo;
        }

        /// <summary>
        /// Ligin Method for User
        /// </summary>
        /// <param name="email">Email of User</param>
        /// <returns>User's details like email, password, user Id</returns>
        public LoginViewModel UserLogin(string email)
        {
            return _AccountRepo.UserLogin(email);
        }

        /// <summary>
        /// Check User Already Register or Not
        /// </summary>
        /// <param name="email">Email of user</param>
        /// <returns>True - If user already register else False</returns>
        public bool IsUserAlreadyRegistered(string email)
        {
            return _AccountRepo.IsUserAlreadyRegistered(email);
        }

        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="registration">User's details like first name, last name, email, password</param>
        /// <returns>True - If successfully register else False</returns>
        public bool RegisterUser(RegistrationViewModel registration)
        {
            return _AccountRepo.RegisterUser(registration);
        }

        /// <summary>
        /// Store OTP
        /// </summary>
        /// <param name="forgotPassword">OTP and user's email Id</param>
        /// <returns>True - If OTP successfully stored else False</returns>
        public bool StoreOTP(ForgotPasswordViewModel forgotPassword)
        {
            return _AccountRepo.StoreOTP(forgotPassword);
        }

        /// <summary>
        /// Verify OTP
        /// </summary>
        /// <param name="forgotPassword">User's Email and their OTP</param>
        /// <returns>"Valid OTP" - If OTP is valid</returns>
        public string VerifyOTP(ForgotPasswordViewModel forgotPassword)
        {
            return _AccountRepo.VerifyOTP(forgotPassword);
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="resetPassword">old password, new password, email of user</param>
        /// <returns>"Changed" - If password changed</returns>
        public string ChangePassword(ResetPasswordViewModel resetPassword)
        {
            return _AccountRepo.ChangePassword(resetPassword);
        }

        /// <summary>
        /// Check If User Have Any Team or Not
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns>True - If User have team else False</returns>
        public bool IsUserHaveAnyTeam(long userID)
        {
            return _AccountRepo.IsUserHaveAnyTeam(userID);
        }
    }
}
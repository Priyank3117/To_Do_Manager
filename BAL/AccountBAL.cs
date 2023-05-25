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

        public LoginViewModel UserLogin(string email)
        {
            return _AccountRepo.UserLogin(email);
        }

        public bool IsUserAlreadyRegistered(string email)
        {
            return _AccountRepo.IsUserAlreadyRegistered(email);
        }

        public bool RegisterUser(RegistrationViewModel registration)
        {
            return _AccountRepo.RegisterUser(registration);
        }

        public bool StoreOTP(ForgotPasswordViewModel forgotPassword)
        {
            return _AccountRepo.StoreOTP(forgotPassword);
        }

        public string VerifyOTP(ForgotPasswordViewModel forgotPassword)
        {
            return _AccountRepo.VerifyOTP(forgotPassword);
        }

        public string ChangePassword(ResetPasswordViewModel resetPassword)
        {
            return _AccountRepo.ChangePassword(resetPassword);
        }

        public bool IsUserHaveAnyTeam(long userID)
        {
            return _AccountRepo.IsUserHaveAnyTeam(userID);
        }
    }
}
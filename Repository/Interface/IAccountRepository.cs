using Entities.ViewModels.AccountViewModels;

namespace Repository.Interface
{
    public interface IAccountRepository
    {
        public bool RegisterUser(RegistrationViewModel registration);

        public bool IsUserAlreadyRegistered(string email);

        public LoginViewModel UserLogin(string email);

        public bool StoreOTP(ForgotPasswordViewModel forgotPassword);

        public string VerifyOTP(ForgotPasswordViewModel forgotPassword);

        public string ChangePassword(ResetPasswordViewModel resetPassword);
    }
}

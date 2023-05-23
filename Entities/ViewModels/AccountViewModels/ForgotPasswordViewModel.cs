using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;

        public int OTP { get; set; }
    }
}

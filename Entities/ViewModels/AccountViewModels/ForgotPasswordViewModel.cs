using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", ErrorMessage = "Enter Valid Email")]
        public string Email { get; set; } = string.Empty;

        public int OTP { get; set; }
    }
}

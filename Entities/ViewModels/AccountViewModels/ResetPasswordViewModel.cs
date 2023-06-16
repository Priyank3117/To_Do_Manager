using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password should contains minimum 8 character, one capital character, one numeric value and one special symbol")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comfirm Password is required")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password should contains minimum 8 character, one capital character, one numeric value and one special symbol")]
        public string NewPassword { get; set; } = string.Empty;


        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
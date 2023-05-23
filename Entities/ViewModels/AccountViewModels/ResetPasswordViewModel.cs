using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Minimum length should be 8 character")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comfirm Password is required")]
        public string NewPassword { get; set; } = string.Empty;
    }
}

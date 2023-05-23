using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        public long UserId { get; set; } = 0;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Minimum length should be 8 character")]
        public string Password { get; set; } = string.Empty;
    }
}

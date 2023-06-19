using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
        public long UserId { get; set; } = 0;

        public string UserName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{1,}$", ErrorMessage = "Enter valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Minimum length should be 8 character")]
        public string Password { get; set; } = string.Empty;
    }
}

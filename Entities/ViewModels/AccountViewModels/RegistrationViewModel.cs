using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class RegistrationViewModel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage ="First Name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage ="Minimum length should be 8 character")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Comfirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match")]
        public string ComfirmPassword { get; set; } = string.Empty;
    }
}

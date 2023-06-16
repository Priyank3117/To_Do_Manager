using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.AccountViewModels
{
    public class RegistrationViewModel
    {
        public long UserId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{1,}$", ErrorMessage = "Enter valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Minimum length should be 8 character")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Password should contains minimum 8 character, one capital character, one numeric value and one special symbol")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comfirm Password is required")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password does not match")]
        public string ComfirmPassword { get; set; } = string.Empty;
    }
}

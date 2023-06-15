using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.UserProfileViewModels
{
    public class UserProfileViewModel
    {
        [Required]
        public long UserId { get; set; } = 0;

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; } = string.Empty;

        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Use letters and space only please")]
        public string? Department { get; set; }

        [RegularExpression(@"^((https?:\/\/)?((www|\w\w)\.)?linkedin\.com\/)((([\w]{2,3})?)|([^\/]+\/(([\w|\d-&#?=])+\/?){1,}))$", ErrorMessage = "Please enter a valid LinkedIn URL")]
        public string? LinkedInURL { get; set; }

        public string? Gender { get; set; }

        public string Avatar { get; set; } = "/images/EmptyProfile.png";
    }
}

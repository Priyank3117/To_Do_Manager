using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Users
    {
        [Key]
        public long UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Avatar { get; set; } = "/images/EmptyProfile.png";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}

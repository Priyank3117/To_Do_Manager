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

        public string? Deparment { get; set; }

        public string? LinkedInURL { get; set; }

        public string Avatar { get; set; } = "/images/EmptyProfile.png";

        public UserGender? Gender { get; set; }

        public enum UserGender
        {
            Male,
            Female
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Notifications> Notifications { get; } = new List<Notifications>();

        public virtual ICollection<Tasks> Tasks { get; } = new List<Tasks>();

        public virtual ICollection<TeamMembers> TeamMembers { get; } = new List<TeamMembers>();
    }
}

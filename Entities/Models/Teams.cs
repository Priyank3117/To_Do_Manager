using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Teams
    {
        [Key]
        public long TeamId { get; set; } 

        public string TeamName { get; set; } = string.Empty;

        public string TeamDescription { get; set; } = string.Empty;

        public virtual ICollection<TeamMembers> TeamMembers { get; } = new List<TeamMembers>();

        public virtual ICollection<Tasks> Tasks { get; } = new List<Tasks>();

        public virtual ICollection<InvitedUsers> InvitedUsers { get; } = new List<InvitedUsers>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Documents> Documents { get; } = new List<Documents>();
    }
}

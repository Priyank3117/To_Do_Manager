using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Teams
    {
        [Key]
        public long TeamId { get; set; }

        public string TeamName { get; set; } = string.Empty;

        public string TeamDescription { get; set; } = string.Empty;

        public virtual ICollection<TeamMembers> TeamMembers { get; } = new List<TeamMembers>();
    }
}

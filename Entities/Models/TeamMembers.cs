using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class TeamMembers
    {
        [Key]
        public long TeamMemberId { get; set; }

        [Display(Name = "Users")]
        public long UserId { get; set; } = 0;

        [ForeignKey("UserId")]
        public virtual Users Users { get; set; } = null!;

        [Display(Name = "Teams")]
        public long TeamId { get; set; }

        [ForeignKey("TeamId")]
        public virtual Teams Teams { get; set; } = new Teams();

        [Column("Role")]
        public Role role { get; set; }

        public enum Role
        {
            TeamMember,
            TeamLeader,
            ReportingPerson
        }

        [Column("Status")]
        public Status status { get; set; }

        public enum Status
        {
            Approved,
            Pending,
            Declined
        }
    }
}

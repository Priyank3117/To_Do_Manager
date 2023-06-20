using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class TeamMembers
    {
        [Key]
        public long TeamMemberId { get; set; }

        [ForeignKey("Users")]
        public long UserId { get; set; } = 0;
        
        public virtual Users Users { get; set; } = null!;

        [ForeignKey("Teams")]
        public long TeamId { get; set; }
        
        public virtual Teams Teams { get; set; } = null!;

        [Column("Role")]
        public Roles Role { get; set; }

        public enum Roles
        {
            TeamMember,
            TeamLeader,
            ReportingPerson
        }

        [Column("Status")]
        public MemberStatus Status { get; set; }

        public enum MemberStatus
        {
            Approved,
            Pending,
            Declined,
            RequestedForLeave
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public long? ReportinPersonUserId { get; set; }

        public string? JoinRequestMessage { get; set; }
    }
}
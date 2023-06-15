using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Tasks
    {
        [Key]
        public long TaskId { get; set; }

        public string TaskName { get; set; } = string.Empty;

        public string? TaskDescription { get; set; }

        public bool TaskStatus { get; set; } = false;

        [ForeignKey("Users")]
        public long UserId { get; set; }

        public virtual Users Users { get; set; } = null!;

        [ForeignKey("Teams")]
        public long TeamId { get; set; }

        public virtual Teams Teams { get; set; } = null!;

        public AssignBy AssignedBy { get; set; }

        public enum AssignBy
        {
            Self,
            TeamLeader,
            ReportingPerson
        }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? IsTodayTask { get; set; }

        public bool IsTaskForToday { get; set; }

        public DateTime StartDate { get; set; } = DateTime.MinValue;

        public DateTime EndDate { get; set; } = DateTime.MaxValue;
    }
}

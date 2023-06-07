using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Notifications
    {
        [Key]
        public long NotificationId { get; set; } = 0;

        [ForeignKey("Users")]
        public long UserId { get; set; }

        public virtual Users Users { get; set; } = null!;

        public string Message { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public enum NotificationType
        {
            SetReportingPerson,
            RemovedReportingPerson,
            ApprovedJoinRequest,
            DeclinedJoinRequest,
            ApprovedLeaveRequest,
            DeclinedLeaveRequest,
            AddedToTeam,
            RemovedFromTeam,
            NewTaskAssigned
        }

        public NotificationType Type { get; set; }           
    }
}

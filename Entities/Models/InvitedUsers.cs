using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class InvitedUsers
    {
        public long InvitedUsersId { get; set; }

        public string Email { get; set; } = string.Empty;

        [ForeignKey("Teams")]
        public long TeamId { get; set; }

        public virtual Teams Teams { get; set; } = null!;
    }
}

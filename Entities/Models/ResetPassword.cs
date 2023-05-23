using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ResetPassword
    {
        [Key]
        public long ResetPasswordId { get; set; }

        public int OTP { get; set; }

        public string Email { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        //[Display(Name = "Users")]
        //public long UserId { get; set; }

        //[ForeignKey("UserId")]
        //public virtual Users User { get; set; } = null!;
    }
}

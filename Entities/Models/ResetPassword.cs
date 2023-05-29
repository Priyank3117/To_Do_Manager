using System.ComponentModel.DataAnnotations;

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
    }
}

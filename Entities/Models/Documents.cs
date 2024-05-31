using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Documents
    {
        [Key]
        public long DocumentId { get; set; }

        [ForeignKey("Teams")]
        public long TeamId { get; set; }

        [ForeignKey("Users")]
        public long UserId { get; set; }

        public virtual Users Users { get; set; } = null!;

        public virtual Teams Teams { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        public string Title {  get; set; } = null!;

        public string Body { get; set; } = null!;
    }
}

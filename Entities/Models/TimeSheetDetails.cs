using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class TimeSheetDetails
    {
        [Key]
        public long TimeSheetDetailsId {  get; set; }

        public string Task { get; set; } = null!;
        public string Note { get; set; } = null!;

        public string ProjectCode { get; set; } = null!;

        
        public string Url { get; set; } = null!;

        [ForeignKey("TimeSheetInputLog")]
        public long TimeSheetInputLogId {  get; set; }

        public virtual TimeSheetInputLog TimeSheetInputLog { get; set; } = null!;

    }
}

using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class TimeSheetInputLog
    {
        [Key]
        public long TimeSheetInputLogID {  get; set; }

        public string FileName { get; set; } = null!;

        public string ProjectType { get; set; } = null!;

        public DateTime CurrentDate { get; set; } = DateTime.Now;

        public bool IsProcesses { get; set; } = false;

        public virtual ICollection<TimeSheetDetails> TimeSheets { get; set; }   = new List<TimeSheetDetails>();

    }
}

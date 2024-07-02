using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class HtmlTemplate
    {
        [Key]
        public long HtmlTemplateId { get; set; }

        public string HtmlTemplateName { get; set; } = string.Empty;

        [ForeignKey("TimeSheetInputLog")]
        public long TimeSheetInputLogId {  get; set; }  

        public virtual TimeSheetInputLog TimeSheetInputLog { get; set; } = null!;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.TimeSheet
{
    public class ProjectDocument
    {
        public long DocumentId { get; set; }
        public string? ProjectType { get; set; } = null!;
        public string? HtmlContent { get; set; } = null!;

    }
}

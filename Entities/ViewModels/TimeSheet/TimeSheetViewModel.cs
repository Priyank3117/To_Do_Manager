using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.TimeSheet
{
    public class TimeSheetViewModel
    {
        public string? ProjectType { get; set; } 

        public IFormFile UploadedFile { get; set; } = null!;

        public string? MessageToShow { get; set; }
        public List<ProjectDocument>? ProjectDocuments { get; set; }
     }
}

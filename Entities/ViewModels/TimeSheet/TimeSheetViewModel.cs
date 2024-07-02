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
        public string? FileName { get; set; }

        public string ProjectType { get; set; } = string.Empty;

        public IFormFile UploadedFile { get; set; } = null!;

        
    }
}

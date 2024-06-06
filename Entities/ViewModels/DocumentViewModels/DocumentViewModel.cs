using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.ViewModels.DocumentViewModels
{

    public class DocumentViewModel
    {
        public string Content { get; set; } = null!;
        public string Title { get; set; } = null!;
        public long DocumentID { get; set; } = 0;
        public long TeamId { get; set; }
         
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TeamName { get; set; }
        public long UserId { get; set; }

    }

}

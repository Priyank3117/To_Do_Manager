using Entities.ViewModels.DocumentViewModels;
using Entities.ViewModels.TimeSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ITimeSheetRepository
    {
        public void AddTimeSheetData(TimeSheetViewModel timeSheetViewModel);

        public TimeSheetViewModel GetDocumentsData();

        public ProjectDocument GetDocumentById(long documnentId);

        public void UpdateContent(ProjectDocument document);

        public string DocumentContent(long documnentId);
    }
}

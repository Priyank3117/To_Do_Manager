using Entities.Migrations;
using Entities.ViewModels.DocumentViewModels;
using Entities.ViewModels.TimeSheet;
using Repository.Interface;
using Repository.Repository;

namespace BAL
{
    public class TimeSheetBAL
    {
        private readonly ITimeSheetRepository _timeSheetRepository;

        public TimeSheetBAL(ITimeSheetRepository timeSheetRepository)
        {
            _timeSheetRepository = timeSheetRepository;
        }


        public void AddTimeSheetData(TimeSheetViewModel timeSheetViewModel)
        {
            _timeSheetRepository.AddTimeSheetData(timeSheetViewModel);
        }

        public TimeSheetViewModel GetDocumentsData()
        {
            return _timeSheetRepository.GetDocumentsData();
        }

        public ProjectDocument GetDocumentById(long documnentId)
        {
            return _timeSheetRepository.GetDocumentById(documnentId);
        }

        public void SaveEditDocument(ProjectDocument projectDocument)
        {
            _timeSheetRepository.UpdateContent(projectDocument);
        }

        public string DocumentContent(long documnentId)
        {
            return _timeSheetRepository.DocumentContent(documnentId);
        }
    }
}

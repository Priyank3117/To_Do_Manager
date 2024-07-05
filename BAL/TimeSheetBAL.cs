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

        /// <summary>
        /// import timesheet excel data and processing it
        /// </summary>
        /// <param name="timeSheetViewModel">User Id</param>
        /// <returns></returns>
        public void AddTimeSheetData(TimeSheetViewModel timeSheetViewModel)
        {
            _timeSheetRepository.AddTimeSheetData(timeSheetViewModel);
        }

        /// <summary>
        /// Get document information
        /// </summary>
        /// <param name="timeSheetViewModel">User Id</param>
        /// <returns></returns>
        public TimeSheetViewModel GetDocumentsData()
        {
            return _timeSheetRepository.GetDocumentsData();
        }

        /// <summary>
        /// import timesheet excel data and processing it
        /// </summary>
        /// <param name="documnentId">User Id</param>
        /// <returns>ProjectDocument viewmodel</returns>
        public ProjectDocument GetDocumentById(long documnentId)
        {
            return _timeSheetRepository.GetDocumentById(documnentId);
        }

        /// <summary>
        /// import timesheet excel data and processing it
        /// </summary>
        /// <param name="projectDocument">User Id</param>
        /// <returns></returns>
        public void SaveEditDocument(ProjectDocument projectDocument)
        {
            _timeSheetRepository.UpdateContent(projectDocument);
        }

        /// <summary>
        /// Get document content form the db
        /// </summary>
        /// <param name="documnentId">User Id</param>
        /// <returns>html string of the document </returns>
        public string DocumentContent(long documnentId)
        {
            return _timeSheetRepository.DocumentContent(documnentId);
        }
    }
}

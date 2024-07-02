using Entities.Migrations;
using Entities.ViewModels.TimeSheet;
using Repository.Interface;

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

        //public TimeSheetViewModel GetDocumentsData()
        //{
           
        //}
    }
}

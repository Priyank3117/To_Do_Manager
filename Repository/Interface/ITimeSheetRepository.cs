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
    }
}

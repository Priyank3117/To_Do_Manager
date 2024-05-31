using Entities.ViewModels.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IDocumentRepository
    {

        public List<AllTeamsViewModel> GetAllAvailableTeams(string searchTerm, long userId);
    }
}

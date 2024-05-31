using Entities.ViewModels.HomeViewModels;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{

    public class DocumentBAL
    {


        private readonly IDocumentRepository _documentRepository;

        public DocumentBAL(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }


        public List<AllTeamsViewModel> GetAllAvailableTeams(string searchTerm, long userId)
        {
            return _documentRepository.GetAllAvailableTeams(searchTerm, userId);
        }


    }
}

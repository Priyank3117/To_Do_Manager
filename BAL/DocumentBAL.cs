using Entities.ViewModels.DocumentViewModels;
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

        public List<AllTeamsViewModel> GetAllAvailableDocs(long userId)
        {
            return _documentRepository.GetAllAvailableDocs(userId);
        }

        public void SaveContent(DocumentViewModel documentViewModel)
        {
            _documentRepository.AddContent(documentViewModel);
        }

        public List<DocumentDisplayViewModel> GetDocuments(long teamId)
        {
            return _documentRepository.GetDocumnets(teamId);
        }

        public DocumentViewModel GetDocumentById(long documnentId, string teamName)
        {
            return _documentRepository.GetDocumentById(documnentId, teamName);
        }

        public string DocumentContent(long documnentId)
        {
            return _documentRepository.DocumentContent(documnentId);
        }


    }
}

using Entities.Models;
using Entities.ViewModels.DocumentViewModels;
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

        public List<AllTeamsViewModel> GetAllAvailableDocs(long userId);

        public void AddContent(DocumentViewModel documentViewModel);

        public List<DocumentDisplayViewModel> GetDocumnets(long teamId);
        public DocumentViewModel GetDocumentById(long documnentId,string teamName);
        public string DocumentContent(long documnentId);
    }
}

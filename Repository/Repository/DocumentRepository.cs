using Entities.Data;
using Entities.Models;
using Entities.ViewModels.DocumentViewModels;
using Entities.ViewModels.HomeViewModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Reflection.Metadata;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repository.Repository
{
    public class DocumentRepository : IDocumentRepository
    {

        private readonly ToDoManagerDBContext _db;

        public DocumentRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }


        public List<AllTeamsViewModel> GetAllAvailableDocs(long userId)
        {
            //i want list of the teammembers that this userid included

            var listOfAvavilableTeam = _db.TeamMembers.Include(s => s.Teams).Where(s => s.UserId == userId).ToList();

            List<DocumentViewModel> listOFDoc= new List<DocumentViewModel>();

            if(listOfAvavilableTeam.Count > 0)
            {
                foreach (var item in listOfAvavilableTeam)
                {
                    var doc = _db.Documents.Where(team => team.TeamId == item.TeamId).Select(Docs => new DocumentViewModel
                    {
                        DocumentID = Docs.DocumentId,
                        Content = Docs.Body,
                        Title = Docs.Title,
                        CreatedDate = Docs.CreatedAt,
                        TeamId = Docs.TeamId,
                        UserId = item.UserId,
                        UpdatedDate = Docs.UpdatedAt
                    });

                    listOFDoc.AddRange(doc);
                }

                //now add that all the document lists of documnet view model in the team
                List<AllTeamsViewModel> allTeamsViewModels =  listOfAvavilableTeam.Select(teams => new AllTeamsViewModel {
                    TeamId = teams.TeamId,
                    TeamName = teams.Teams.TeamName,
                    DocumentViewModels = listOFDoc
                }).ToList();

                return allTeamsViewModels;
            }
            else
            {
                return new List<AllTeamsViewModel>();  
            }

        }

        public void AddContent(DocumentViewModel documentViewModel)
        {
            if (documentViewModel.DocumentID == 0)
            {
            Documents documents = new Documents();

            documents.UserId = documentViewModel.UserId;
            documents.TeamId = documentViewModel.TeamId;
            documents.Title = documentViewModel.Title;
            documents.Body = documentViewModel.Content;
                documents.CreatedAt = DateTime.Now;
                _db.Add(documents);
                _db.SaveChanges();
            }
            else
            {
                Documents documents = _db.Documents.FirstOrDefault(x => x.DocumentId == documentViewModel.DocumentID);
                documents.Title = documentViewModel.Title;
                documents.Body = documentViewModel.Content;
                documents.UpdatedAt = DateTime.Now;
                _db.Documents.Update(documents);
                _db.SaveChanges();
            }


        }

        public List<DocumentDisplayViewModel> GetDocumnets(long teamId)
        {
            var documents = _db.Documents.Where(s => s.TeamId == teamId).ToList();

            if (documents.Count > 0)
            {
                var listOfDoc = documents.Select(s => new DocumentDisplayViewModel()
                {
                    DocId = s.DocumentId,
                    Title = s.Title,
                }).ToList();

                return listOfDoc;
            }
            else
            {
                return new List<DocumentDisplayViewModel>();
            }
        }

        public DocumentViewModel GetDocumentById(long documnentId,string teamName)
        {
            Documents documents = _db.Documents.FirstOrDefault(s => s.DocumentId == documnentId);
            return new DocumentViewModel
            {
                DocumentID = documents.DocumentId,
                Title = documents.Title,
                Content = documents.Body,
                TeamId = documents.TeamId,
                UserId = documents.UserId,
                TeamName = teamName
            };
        }

        public string DocumentContent(long documnentId)
        {
            return _db.Documents.Where(s => s.DocumentId == documnentId).Select(s => s.Body).FirstOrDefault();
        }
    }
}

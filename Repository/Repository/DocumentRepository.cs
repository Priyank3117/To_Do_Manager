using Entities.Data;
using Entities.ViewModels.HomeViewModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
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
        public List<AllTeamsViewModel> GetAllAvailableTeams(string searchTerm, long userId)
        {
            //i want list of the teammembers that this userid included

            var list = _db.TeamMembers.Include(s => s.Teams).Where(s  => s.UserId == userId).ToList();

            if (list.Count > 0)
            {
                var teams = list.Select(team => new AllTeamsViewModel()
                {
                    TeamId = team.TeamId,
                    TeamName = team.Teams.TeamName,
                    Status = list.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString()
                }).ToList();

                return teams;

            }
            else
            {
             return new List<AllTeamsViewModel>();
            }

            
        }
    }
}

using Entities.Data;
using Entities.Models;
using Entities.ViewModels.HomeViewModels;
using Repository.Interface;

namespace Repository.Repository
{
    public class HomeRepository: IHomeRepository
    {
        private readonly ToDoManagerDBContext _db;

        public HomeRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        public long CreateTeam(TeamViewModel team)
        {
            Teams teams = new()
            {
                TeamName = team.TeamName,
                TeamDescription = team.TeamDescription
            };

            _db.Add(teams);
            _db.SaveChanges();

            return teams.TeamId;
        }

        public bool AddUserToTeam(string email, long teamId)
        {
            var isUserIDExist = _db.Users.Where(user => user.Email == email).Select( user => user.UserId).FirstOrDefault();

            if(isUserIDExist != 0)
            {
                TeamMembers teamMembers = new()
                {
                    TeamId = teamId,
                    UserId = isUserIDExist,
                    role = TeamMembers.Role.TeamMember,
                    status = TeamMembers.Status.Approved
                };

                _db.Add(teamMembers);
                _db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}

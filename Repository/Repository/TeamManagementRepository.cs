using Entities.Data;
using Entities.ViewModels.TeamManagement;
using Repository.Interface;

namespace Repository.Repository
{
    public class TeamManagementRepository: ITeamManagementRepository
    {
        private readonly ToDoManagerDBContext _db;

        public TeamManagementRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId)
        {
            var query = _db.TeamMembers.AsQueryable();
            var users = _db.Users.AsQueryable();
                
               var teams = query.Where(team => team.UserId == userId).Select(team => new TeamManagementViewModel()
            {
                TeamId = team.TeamId,
                TeamName = team.Teams.TeamName,
                TeamDescription = team.Teams.TeamDescription,
                Role = team.Role.ToString(),
                TeamMembers = query.Where( teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId != userId && teamMember.Status == Entities.Models.TeamMembers.MemberStatus.Approved).Select( user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                    ReportingPersonUserId = user.ReportinPersonUserId,
                    ReportingPersonUserName = user.ReportinPersonUserId != null ? users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.FirstName + " " + users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.LastName : null!,
                    ReportingPersonAvatar = users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.Avatar
                }).ToList(),
                JoinRequests = team.Role.ToString() == "TeamLeader" ? query.Where(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId != userId && teamMember.Status == Entities.Models.TeamMembers.MemberStatus.Pending).Select( user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                }).ToList() : new List<UserDetailOfTeam>(),
            }).ToList();

            return teams;
        }
    }
}

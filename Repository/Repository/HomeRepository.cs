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

            TeamMembers teamMembers = new()
            {
                TeamId = teams.TeamId,
                UserId = team.TeamLeaderUserId,
                Role = TeamMembers.Roles.TeamLeader,
                Status = TeamMembers.MemberStatus.Approved
            };

            _db.Add(teamMembers);
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
                    Role = TeamMembers.Roles.TeamMember,
                    Status = TeamMembers.MemberStatus.Approved
                };

                _db.Add(teamMembers);
                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        public List<AllTeamsViewModel> GetAllTeams(string searchTerm, long userId)
        {
            var query = _db.Teams.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where( team => team.TeamName.ToLower().Contains(searchTerm.ToLower()));
            }

            return query.Select(team => new AllTeamsViewModel()
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Status = team.TeamMembers.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId).Status.ToString(),
            }).ToList();
        }

        public TeamViewModel GetTeamDetails(long teamId)
        {
            return _db.Teams.Where(team => team.TeamId == teamId).Select(team => new TeamViewModel()
            {
                TeamId = team.TeamId,
                TeamDescription = team.TeamDescription,
                TeamName = team.TeamName
            }).FirstOrDefault()!;
        }

        public bool RequestToJoinTeam(TeamMemberViewModel userRequest)
        {
            if(userRequest != null)
            {
                TeamMembers teamMembers = new()
                {
                    TeamId = userRequest.TeamId,
                    UserId = userRequest.UserId,
                    Role = TeamMembers.Roles.TeamMember,
                    Status = TeamMembers.MemberStatus.Pending
                };

                _db.Add(teamMembers);
                _db.SaveChanges();

                return true;
            }
            return false;
        }

        public List<TodayTasksViewModel> GetAllTodayTasks(long userId)
        {
            var query = _db.Tasks.Where(task => task.UserId == userId).AsQueryable();

            var todatTasks = query.Select(task => new TodayTasksViewModel()
            {
                TeamId = task.TeamId,
                TeamName = task.Teams.TeamName,
                UserId = task.UserId,
                TodayTasks = query.Select(task => new TaskDetailViewModel()
                {
                    TaskId = task.TaskId,
                    TaskName = task.TaskName,
                    IsCompleted = task.TaskStatus
                }).ToList(),
                TeamMembersTaasks = query.Select(task => task.Teams.TeamMembers.Where(teamMember => teamMember.Role.ToString() == "TeamMember" && teamMember.TeamId == task.TeamId).Select(teamMember => new TeamMembersTaskDetails()
                {
                    UserId = teamMember.UserId,
                    TaskId = task.TaskId,
                    TaskName = task.TaskName,
                    Avatar = teamMember.Users.Avatar,
                    UserName = teamMember.Users.FirstName + " " + teamMember.Users.LastName,
                    IsCompleted = task.TaskStatus,
                    TodayTasks = query.Select(task => new TaskDetailViewModel()
                    {
                        TaskId = task.TaskId,
                        TaskName = task.TaskName,
                        IsCompleted = task.TaskStatus
                    }).ToList()
                })).

            });
        }
    }
}

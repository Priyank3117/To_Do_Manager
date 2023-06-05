using Entities.Data;
using Entities.Models;
using Entities.ViewModels.AllTasksViewModel;
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
                InvitedUsers invitedUsers = new();
                invitedUsers.TeamId = teamId;
                invitedUsers.Email = email;

                _db.Add(invitedUsers);
                _db.SaveChanges();

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
                Status = team.TeamMembers.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString(),
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
            List<TodayTasksViewModel> teams = new();

            var query = _db.Tasks.AsQueryable();
            var totalTeams = _db.TeamMembers.Where(teamMembers =>  teamMembers.UserId == userId).Select( teamMember => teamMember.TeamId).ToList();

            foreach (var teamId in totalTeams)
            {
                TodayTasksViewModel team = new();
                var myTasks = query.Where(task => task.UserId == userId && task.TeamId == teamId && task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true);
                team.TeamId = teamId;
                team.TeamName = query.Where(p => p.Teams.TeamId == teamId).Select(p => p.Teams.TeamName).FirstOrDefault()!; 
                team.UserId = userId;
                team.Role = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId)!.Role.ToString();

                foreach (var mytask in myTasks)
                {
                    TaskDetailViewModel taskDetailViewModel = new();
                    taskDetailViewModel.TaskName = mytask.TaskName;
                    taskDetailViewModel.IsCompleted = mytask.TaskStatus;
                    taskDetailViewModel.TaskId = mytask.TaskId;
                    taskDetailViewModel.UserId = userId;
                    taskDetailViewModel.TeamId = teamId;

                    team.TodayTasks.Add(taskDetailViewModel);
                }

                if (team.Role != "TeamMember")
                {
                    var totalMembers = query.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();

                    foreach (var memberTask in totalMembers)
                    {
                        var teamMemberTasks = query.Where(task => task.UserId == memberTask && task.TeamId == teamId && task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true);

                        if (teamMemberTasks.Any())
                        {
                            TeamMembersTaskDetails teamMembersTaskDetails = new();
                            teamMembersTaskDetails.Avatar = teamMemberTasks.Select(p => p.Users.Avatar).FirstOrDefault()!.ToString();
                            teamMembersTaskDetails.UserName = teamMemberTasks.Select(p => p.Users.FirstName).FirstOrDefault()!.ToString() + " " + teamMemberTasks.Select(p => p.Users.LastName).FirstOrDefault()!.ToString();

                            foreach (var task in teamMemberTasks)
                            {
                                TaskDetailViewModel taskDetailViewModel = new();
                                taskDetailViewModel.TaskName = task.TaskName;
                                taskDetailViewModel.IsCompleted = task.TaskStatus;
                                taskDetailViewModel.TaskId = task.TaskId;
                                taskDetailViewModel.UserId = memberTask;
                                taskDetailViewModel.TeamId = teamId;

                                teamMembersTaskDetails.TodayTasks.Add(taskDetailViewModel);
                            }
                            team.TeamMembersTaasks.Add(teamMembersTaskDetails);
                        }
                    }
                }

                teams.Add(team);
            }

            return teams;
        }

        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            var query = _db.TeamMembers.AsQueryable();

            if(query.Any(teamMember => teamMember.UserId == userId && teamMember.Role == TeamMembers.Roles.TeamLeader))
            {
                return query.Where(teamMember => teamMember.TeamId == teamId).Select(task => new ListOfUsers()
                {
                    UserId = task.UserId,
                    UserName = task.Users.FirstName + " " + task.Users.LastName
                }).ToList();
            }
            else
            {
                return new List<ListOfUsers>();
            }
        }

        public bool AddTask(TaskDetailViewModel task)
        {
            if(task != null)
            {
                if(task.StartDate == null && task.EndDate == null)
                {
                    Tasks addTask = new()
                    {
                        TaskStatus = false,
                        TeamId = task.TeamId,
                        TaskName = task.TaskName,
                        TaskDescription = task.TaskDescription,
                        UserId = task.UserId,
                        IsTaskForToday = true,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now
                    };

                    _db.Add(addTask);
                    _db.SaveChanges();
                }
                else
                {
                    Tasks addTask = new()
                    {
                        TaskStatus = false,
                        TeamId = task.TeamId,
                        TaskName = task.TaskName,
                        TaskDescription = task.TaskDescription,
                        UserId = task.UserId,
                        IsTaskForToday = true,
                        StartDate = (DateTime)task.StartDate!,
                        EndDate = (DateTime)task.EndDate!
                    };

                    _db.Add(addTask);
                    _db.SaveChanges();
                }
                return true;
            }
            return false;
        }

        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel taskDetail)
        {
            var task = _db.Tasks.FirstOrDefault(task => task.TaskId == taskDetail.TaskId && task.TeamId == taskDetail.TeamId && task.UserId == taskDetail.UserId)!;
            
            if(task.TaskStatus == true)
            {
                task.TaskStatus = false;
                _db.Update(task);
                _db.SaveChanges();

                return false;
            }
            else
            {
                task.TaskStatus = true;
                _db.Update(task);
                _db.SaveChanges();

                return true;
            }
        }

        public TaskDetailViewModel GetTaskDetails(long taskId)
        {
            if(taskId != 0)
            {
                var taskdetail = _db.Tasks.Where(t => t.TaskId == taskId).Select( task => new TaskDetailViewModel()
                {
                    TaskDescription = task.TaskDescription,
                    TaskId = taskId,
                    TaskName = task.TaskName,
                    TeamName = task.Teams.TeamName,
                    StartDateForDisplay = task.StartDate.ToString("yyyy-MM-dd"),
                    EndDateForDisplay = task.EndDate.ToString("yyyy-MM-dd"),
                    IsCompleted = task.TaskStatus
                }).FirstOrDefault();

                if(taskdetail != null)
                {
                    return taskdetail;
                }
                else
                {
                    return new TaskDetailViewModel();
                }
            }
            else
            {
                return new TaskDetailViewModel();
            }
        }
    }
}

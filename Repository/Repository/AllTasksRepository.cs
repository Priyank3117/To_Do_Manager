using Entities.Data;
using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;
using Repository.Interface;

namespace Repository.Repository
{
    public class AllTasksRepository : IAllTasksRepository
    {
        private readonly ToDoManagerDBContext _db;

        public AllTasksRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        public List<AllTasksOfAllTeams> GetAllTasks(long userId, string searchTerm)
        {
            List<AllTasksOfAllTeams> teams = new();

            var query = _db.Tasks.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(task => task.Teams.TeamName.ToLower().Contains(searchTerm.ToLower()) || task.TaskName.ToLower().Contains(searchTerm.ToLower()));
            }

            //query = query.OrderByDescending(task => task.IsTodayTask);            
            var totalTeams = _db.TeamMembers.Where(teamMembers => teamMembers.UserId == userId && teamMembers.Status == Entities.Models.TeamMembers.MemberStatus.Approved).Select(teamMember => teamMember.TeamId).ToList();

            foreach (var teamId in totalTeams)
            {
                AllTasksOfAllTeams team = new();
                var myTasks = query.Where(task => task.UserId == userId && task.TeamId == teamId);
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
                    taskDetailViewModel.IsTodayTask = mytask.StartDate.Date <= DateTime.Now.Date && mytask.EndDate >= DateTime.Now.Date && mytask.IsTaskForToday == true ? true : false;

                    team.MyTasks.Add(taskDetailViewModel);
                }

                if (team.Role != "TeamMember")
                {
                    var totalMembers = query.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();

                    foreach (var memberTask in totalMembers)
                    {
                        var teamMemberTasks = query.Where(task => task.UserId == memberTask && task.TeamId == teamId);

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
                                taskDetailViewModel.IsTodayTask = task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true ? true : false;

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

        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail)
        {
            var task = _db.Tasks.FirstOrDefault(task => task.TaskId == taskDetail.TaskId && task.TeamId == taskDetail.TeamId && task.UserId == taskDetail.UserId)!;

            if (task.IsTaskForToday == false)
            {
                task.IsTaskForToday = true;
                task.StartDate = DateTime.Now;
                task.EndDate = DateTime.Now;

                _db.Update(task);
                _db.SaveChanges();

                return true;
            }
            else
            {
                task.IsTaskForToday = false;

                _db.Update(task);
                _db.SaveChanges();

                return false;
            }
        }

        public List<AllTaskForCalenderView> GetTasksForCalenderView(long userId)
        {
            var query = _db.Tasks.AsQueryable();

            var totalTeams = query.Where(task => task.UserId == userId).Select(task => task.TeamId).Distinct().ToList();

            List<AllTaskForCalenderView> AllTasks = new();

            foreach (var teamId in totalTeams)
            {
                var myTasks = query.Where(task => task.UserId == userId && task.TeamId == teamId);

                foreach (var mytask in myTasks)
                {
                    AllTaskForCalenderView task = new();
                    task.TaskName = mytask.TaskName;
                    task.IsCompleted = mytask.TaskStatus;
                    task.StartDate = mytask.StartDate.ToString("yyyy-MM-dd");
                    task.EndDate = mytask.EndDate.ToString("yyyy-MM-dd");

                    AllTasks.Add(task);
                }

                if (_db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId)!.Role.ToString() != "TeamMember")
                {
                    var totalMembers = query.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();

                    foreach (var memberTask in totalMembers)
                    {
                        var teamMemberTasks = query.Where(task => task.UserId == memberTask && task.TeamId == teamId);

                        if (teamMemberTasks.Any())
                        {
                            foreach (var task in teamMemberTasks)
                            {
                                AllTaskForCalenderView taskOfMember = new();
                                taskOfMember.TaskName = task.TaskName;
                                taskOfMember.IsCompleted = task.TaskStatus;
                                taskOfMember.StartDate = task.StartDate.ToString("yyyy-MM-dd");
                                taskOfMember.EndDate = task.EndDate.ToString("yyyy-MM-dd");

                                AllTasks.Add(taskOfMember);
                            }
                        }
                    }
                }
            }
            return AllTasks;
        }
    }
}

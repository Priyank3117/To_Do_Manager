﻿using Entities.Data;
using Entities.Models;
using Entities.ViewModels.HomeViewModels;
using Repository.Interface;

namespace Repository.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ToDoManagerDBContext _db;

        public HomeRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Create Team
        /// </summary>
        /// <param name="team">Team Details like Team Name, Team Discription and Team Members</param>
        /// <returns>True - If successfully team created else False</returns>
        public long CreateTeam(TeamViewModel team)
        {
            Teams teams = new()
            {
                TeamName = team.TeamName.Trim(),
                TeamDescription = team.TeamDescription.Trim()
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

        /// <summary>
        /// Add User to Team
        /// </summary>
        /// <param name="email">Email Id of user</param>
        /// <param name="teamId">Team Id in which user will be added</param>
        /// <returns>True - If user already register then directly added to team else False while user not registered then email sent to this user and this add this user's details in the Invitation Table for future purpose</returns>
        public bool AddUserToTeam(string email, long teamId)
        {
            var isUserIDExist = _db.Users.Where(user => user.Email == email).Select(user => user.UserId).FirstOrDefault();

            if (isUserIDExist != 0)
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

        /// <summary>
        /// Get Available All Teams with Team Name
        /// </summary>
        /// <param name="searchTerm">Search Keyword</param>
        /// <param name="userId"></param>
        /// <returns>List of all available teams</returns>
        //public List<AllTeamsViewModel> GetAllTeamsTemp(string searchTerm, long userId)
        //{
        //    var query = _db.Teams.AsQueryable();
        //    var userQuery = _db.Users.AsQueryable();
        //    var getDomain = userQuery.Where(user => user.UserId == userId).Select(user => user.Email.Substring(user.Email.LastIndexOf("@") + 1));

        //    if (!string.IsNullOrEmpty(searchTerm))
        //    {
        //        query = query.Where(team => team.TeamName.ToLower().Contains(searchTerm.ToLower()));
        //    }

        //    var x = GetAllRelatedTeams(getDomain.FirstOrDefault());

        //    List<AllTeamsViewModel> teams = new();            

        //    //var y = x.Select(team => new AllTeamsViewModel()
        //    //{
        //    //    TeamId = team.TeamId,
        //    //    TeamName = team.TeamName,
        //    //    Status = team.TeamMembers.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString(),
        //    //}).ToList();

        //    foreach (var team in x)
        //    {
        //        AllTeamsViewModel tempTeam = new();

        //        tempTeam.TeamId = team.TeamId;
        //        tempTeam.TeamName = team.TeamName;
        //        tempTeam.Status = FindStatus(team.TeamId, userId);

        //        teams.Add(tempTeam);
        //    }

        //    return teams;
        //}

        public List<AllTeamsViewModel> GetAllTeamsTemp(string searchTerm, long userId)
        {
            var query = _db.Teams.AsQueryable();
            

            var userQuery = _db.Users.AsQueryable();

            var emailDomainOfUser = userQuery.FirstOrDefault(user => user.UserId == userId)!.Email;
            emailDomainOfUser = emailDomainOfUser.Substring(emailDomainOfUser.LastIndexOf("@") + 1);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(team => team.TeamName.ToLower().Contains(searchTerm.ToLower()));
            }

            return query.Select(team => new AllTeamsViewModel()
            {
                TeamId = team.TeamId,
                TeamName = team.TeamName,
                Status = team.TeamMembers.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString(),
            }).ToList();
        }

        public List<AllTeamsViewModel> GetAllTeams(string searchTerm, long userId)
        {
            var query = _db.TeamMembers.AsQueryable();

            var userQuery = _db.Users.AsQueryable();

            var emailDomainOfUser = userQuery.FirstOrDefault(user => user.UserId == userId)!.Email;
            emailDomainOfUser = emailDomainOfUser.Substring(emailDomainOfUser.LastIndexOf("@") + 1);            

            var realatedTeams = query.Where(team => team.Users.Email.Contains(emailDomainOfUser) && team.Role == TeamMembers.Roles.TeamLeader);

            if(realatedTeams.Any())
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    realatedTeams = realatedTeams.Where(team => team.Teams.TeamName.ToLower().Contains(searchTerm.ToLower()));
                }

                var teams = realatedTeams.Select(team => new AllTeamsViewModel()
                {
                    TeamId = team.TeamId,
                    TeamName = team.Teams.TeamName,
                    Status = query.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString()
                }).ToList();

                return teams;
            }
            else
            {
                return GetTeamsOfOtherDomain(searchTerm, userId);
            }            
        }

        public List<AllTeamsViewModel> GetTeamsOfOtherDomain(string searchTerm, long userId)
        {
            List<AllTeamsViewModel> teams = new();

            var query = _db.TeamMembers.AsQueryable();

            var realatedTeamsByEmail = query.Where(team => team.Users.Email.ToLower().Contains(searchTerm) && team.Role == TeamMembers.Roles.TeamLeader);

            if (realatedTeamsByEmail.Any())
            {
                teams.AddRange(realatedTeamsByEmail.Select(team => new AllTeamsViewModel()
                {
                    TeamId = team.TeamId,
                    TeamName = team.Teams.TeamName,
                    Status = query.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString()
                }).ToList());
            }            

            var realatedTeamsByUserName = query.Where(team => (team.Users.FirstName + " " + team.Users.LastName).ToLower().Contains(searchTerm) && team.Role == TeamMembers.Roles.TeamLeader);

            if (realatedTeamsByUserName.Any())
            {
                teams.AddRange(realatedTeamsByUserName.Select(team => new AllTeamsViewModel()
                {
                    TeamId = team.TeamId,
                    TeamName = team.Teams.TeamName,
                    Status = query.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString()
                }).ToList());
            }            

            var realatedTeamsByTeamName = query.Where(team => team.Teams.TeamName.ToLower().Contains(searchTerm) && team.Role == TeamMembers.Roles.TeamLeader);

            if (realatedTeamsByTeamName.Any())
            {
                teams.AddRange(realatedTeamsByTeamName.Select(team => new AllTeamsViewModel()
                {
                    TeamId = team.TeamId,
                    TeamName = team.Teams.TeamName,
                    Status = query.FirstOrDefault(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId == userId)!.Status.ToString()
                }).ToList());
            }  

            return teams.DistinctBy(team => team.TeamId).ToList();
        }

        /// <summary>
        /// Get Teams Details
        /// </summary>
        /// <param name="teamId">TeamId</param>
        /// <returns>All Tasks with Team Name</returns>
        public TeamViewModel GetTeamDetails(long teamId)
        {
            return _db.Teams.Where(team => team.TeamId == teamId).Select(team => new TeamViewModel()
            {
                TeamId = team.TeamId,
                TeamDescription = team.TeamDescription,
                TeamName = team.TeamName
            }).FirstOrDefault()!;
        }

        /// <summary>
        /// Request to Join Team
        /// </summary>
        /// <param name="userRequest">UserId, TeamId</param>
        /// <returns>True - If successfully requested alse False</returns>
        public bool RequestToJoinTeam(TeamMemberViewModel userRequest)
        {
            if (userRequest != null)
            {
                TeamMembers teamMembers = new()
                {
                    TeamId = userRequest.TeamId,
                    UserId = userRequest.UserId,
                    Role = TeamMembers.Roles.TeamMember,
                    Status = TeamMembers.MemberStatus.Pending,
                    JoinRequestMessage = userRequest.JoinRequestMessage
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

        /// <summary>
        /// Get All Today's Task for To-Do List Page
        /// </summary>
        /// <param name="userId">User Id of user</param>
        /// <returns>List of all today's task of this user with task details like task status whether it is completed or not and task name</returns>
        public List<TodayTasksViewModel> GetAllTodayTasks(long userId)
        {
            List<TodayTasksViewModel> teams = new();

            var query = _db.Tasks.AsQueryable();
            var teamMemberQuery = _db.TeamMembers.AsQueryable();
            var totalTeams = teamMemberQuery.Where(teamMembers => teamMembers.UserId == userId && (teamMembers.Status == TeamMembers.MemberStatus.Approved || teamMembers.Status == TeamMembers.MemberStatus.RequestedForLeave)).Select(teamMember => teamMember.TeamId).ToList();

            foreach (var teamId in totalTeams)
            {
                TodayTasksViewModel team = new();
                var myTasks = query.Where(task => task.UserId == userId && task.TeamId == teamId && task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true);
                team.TeamId = teamId;
                team.TeamName = teamMemberQuery.Where(p => p.TeamId == teamId).Select(p => p.Teams.TeamName).FirstOrDefault()!;
                team.UserId = userId;
                team.Role = teamMemberQuery.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId)!.Role.ToString();

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

                List<long> totalMembers = new();

                if (team.Role == "TeamLeader")
                {
                    totalMembers = teamMemberQuery.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();
                }
                else if (team.Role == "ReportingPerson")
                {
                    totalMembers = teamMemberQuery.Where(task => task.UserId != userId && task.TeamId == teamId && task.ReportinPersonUserId == userId).Select(p => p.UserId).Distinct().ToList();
                }

                foreach (var memberTask in totalMembers)
                {
                    var teamMemberTasks = query.Where(task => task.UserId == memberTask && task.TeamId == teamId && task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true);

                    if (teamMemberTasks.Any())
                    {
                        TeamMembersTaskDetails teamMembersTaskDetails = new();
                        teamMembersTaskDetails.Avatar = teamMemberTasks.Select(p => p.Users.Avatar).FirstOrDefault()!.ToString();
                        teamMembersTaskDetails.UserId = teamMemberTasks.Select(p => p.Users.UserId).FirstOrDefault();
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

                teams.Add(team);
            }

            return teams;
        }

        public List<AllTodayTasksForListView> GetAllTodayTasksForListView(long userId)
        {
            var query = _db.Tasks.AsQueryable();

            List<AllTodayTasksForListView> allTasks = new();

            var myTasks = query.Where(task => task.UserId == userId && task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true);

            allTasks.AddRange(myTasks.Select(task => new AllTodayTasksForListView()
            {
                TaskId = task.TaskId,
                TeamId = task.TeamId,
                UserId = task.UserId,
                TaskName = task.TaskName,
                TaskStatus = task.TaskStatus,
                TeamName = task.Teams.TeamName,
                Deadline = task.EndDate.Date.ToString("dd/MM/yyyy"),
                IsMyTask = true
            }).ToList());

            var teamMemberQuery = _db.TeamMembers.AsQueryable();
            var totalTeams = teamMemberQuery.Where(teamMembers => teamMembers.UserId == userId && (teamMembers.Status == TeamMembers.MemberStatus.Approved || teamMembers.Status == TeamMembers.MemberStatus.RequestedForLeave)).Select(teamMember => teamMember.TeamId).ToList();

            foreach (var teamId in totalTeams)
            {
                var userRole = teamMemberQuery.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId)!.Role.ToString();

                List<long> totalMembers = new();

                if (userRole == "TeamLeader")
                {
                    totalMembers = teamMemberQuery.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();
                }
                else if (userRole == "ReportingPerson")
                {
                    totalMembers = teamMemberQuery.Where(task => task.UserId != userId && task.TeamId == teamId && task.ReportinPersonUserId == userId).Select(p => p.UserId).Distinct().ToList();
                }

                AllTodayTasksForListView taskInfo = new();

                foreach (var memberUserId in totalMembers)
                {
                    var memberTask = query.Where(task => task.UserId == memberUserId && task.TeamId == teamId && task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true);

                    allTasks.AddRange(memberTask.Select(task => new AllTodayTasksForListView()
                    {
                        TaskId = task.TaskId,
                        TeamId = task.TeamId,
                        UserId = task.UserId,
                        TaskName = task.TaskName,
                        TaskStatus = task.TaskStatus,
                        TeamName = task.Teams.TeamName,
                        Deadline = task.EndDate.Date.ToString("dd/MM/yyyy"),
                        Avatar = task.Users.Avatar,
                        UserName = task.Users.FirstName + " " + task.Users.LastName,
                        IsMyTask = false
                    }).ToList());
                }
            }

            return allTasks.OrderBy(team => team.TeamName).ToList();
        }

        /// <summary>
        /// Get Data for Add Task
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>List of User's UserName with User Id</returns>
        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            var query = _db.TeamMembers.AsQueryable();

            var memberRole = query.FirstOrDefault(teamMember => teamMember.TeamId == teamId && teamMember.UserId == userId)!;

            if (memberRole.Role == TeamMembers.Roles.TeamLeader)
            {
                return query.Where(teamMember => teamMember.TeamId == teamId).Select(task => new ListOfUsers()
                {
                    UserId = task.UserId,
                    UserName = task.Users.FirstName + " " + task.Users.LastName,
                    Avatar = task.Users.Avatar
                }).ToList();
            }
            else if (memberRole.Role == TeamMembers.Roles.ReportingPerson)
            {
                var allUnderMember = query.Where(teamMember => teamMember.TeamId == teamId && teamMember.ReportinPersonUserId == userId).Select(p => new ListOfUsers()
                {
                    UserName = p.Users.FirstName + " " + p.Users.LastName,
                    UserId = p.UserId,
                    Avatar = p.Users.Avatar
                }).ToList();

                var reportingPerson = query.Where(teamMember => teamMember.TeamId == teamId && teamMember.UserId == memberRole.UserId).Select(p => new ListOfUsers()
                {
                    UserName = p.Users.FirstName + " " + p.Users.LastName,
                    UserId = p.UserId,
                    Avatar = p.Users.Avatar
                }).FirstOrDefault()!;

                allUnderMember.Add(reportingPerson);

                return allUnderMember;
            }
            else
            {
                return new List<ListOfUsers>();
            }
        }

        /// <summary>
        /// Add Task In Team
        /// </summary>
        /// <param name="task">Task Name, Team Id and User Id</param>
        /// <returns>True - If successfully Add Task alse False</returns>
        public bool AddTask(TaskDetailViewModel task)
        {
            if (task != null)
            {
                var fromUser = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == task.FromUserId && teamMember.TeamId == task.TeamId)!.Role;

                Tasks.AssignBy assignBy = new();

                if (fromUser == TeamMembers.Roles.ReportingPerson)
                {
                    assignBy = Tasks.AssignBy.ReportingPerson;
                }
                else if (fromUser == TeamMembers.Roles.TeamLeader)
                {
                    assignBy = Tasks.AssignBy.TeamLeader;
                }
                else
                {
                    assignBy = Tasks.AssignBy.Self;
                }

                if (task.StartDate == null && task.EndDate == null)
                {
                    Tasks addTask = new()
                    {
                        TaskStatus = false,
                        TeamId = task.TeamId,
                        TaskName = task.TaskName.Trim(),
                        TaskDescription = task.TaskDescription?.Trim(),
                        UserId = task.UserId,
                        IsTaskForToday = true,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        AssignedBy = assignBy
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
                        TaskName = task.TaskName.Trim(),
                        TaskDescription = task.TaskDescription?.Trim(),
                        UserId = task.UserId,
                        IsTaskForToday = true,
                        StartDate = (DateTime)task.StartDate!,
                        EndDate = (DateTime)task.EndDate!,
                        AssignedBy = assignBy
                    };

                    _db.Add(addTask);
                    _db.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Mark Task as Complete Or Uncomplete
        /// </summary>
        /// <param name="task">Task Id, Team Id and User Id</param>
        /// <returns>True - If successful alse False</returns>
        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel taskDetail)
        {
            var task = _db.Tasks.FirstOrDefault(task => task.TaskId == taskDetail.TaskId && task.TeamId == taskDetail.TeamId && task.UserId == taskDetail.UserId)!;

            if (task.TaskStatus == true)
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

        /// <summary>
        /// Get Task Details
        /// </summary>
        /// <param name="taskId">Task Id</param>
        /// <returns>Task Details like - Task Name, Task Description, Start Date, End Date and IsCompleted or Not</returns>
        public TaskDetailViewModel GetTaskDetails(long taskId)
        {
            if (taskId != 0)
            {
                var taskdetail = _db.Tasks.Where(t => t.TaskId == taskId).Select(task => new TaskDetailViewModel()
                {
                    TaskDescription = task.TaskDescription,
                    TaskId = taskId,
                    TaskName = task.TaskName,
                    TeamName = task.Teams.TeamName,
                    StartDateForDisplay = task.StartDate.ToString("yyyy-MM-dd"),
                    EndDateForDisplay = task.EndDate.ToString("yyyy-MM-dd"),
                    IsCompleted = task.TaskStatus
                }).FirstOrDefault();

                if (taskdetail != null)
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

        /// <summary>
        /// Edit Task In Task Description OffCanvas
        /// </summary>
        /// <param name="task">Task Details like - Task Id, Task Name, Task Description, Start Date, End Date and Task Status</param>
        /// <returns>True - If task successfully edited else False</returns>
        public bool EditTask(TaskDetailViewModel task)
        {
            if (task != null && task.TaskId != 0)
            {
                var taskDetails = _db.Tasks.FirstOrDefault(tasks => tasks.TaskId == task.TaskId);

                if (taskDetails != null)
                {
                    taskDetails.TaskName = task.TaskName.Trim();
                    taskDetails.TaskDescription = task.TaskDescription?.Trim();
                    taskDetails.StartDate = (DateTime)task.StartDate!;
                    taskDetails.EndDate = (DateTime)task.EndDate!;
                    taskDetails.TaskStatus = task.IsCompleted;

                    _db.SaveChanges();
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

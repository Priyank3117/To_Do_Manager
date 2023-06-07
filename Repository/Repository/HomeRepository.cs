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
        public List<AllTeamsViewModel> GetAllTeams(string searchTerm, long userId)
        {
            var query = _db.Teams.AsQueryable();

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
                    Status = TeamMembers.MemberStatus.Pending
                };

                _db.Add(teamMembers);
                _db.SaveChanges();

                return true;
            }
            return false;
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
                    totalMembers = query.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();
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

        /// <summary>
        /// Get Data for Add Task
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>List of User's UserName with User Id</returns>
        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            var query = _db.TeamMembers.AsQueryable();

            if (query.Any(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId && teamMember.Role == TeamMembers.Roles.TeamLeader))
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

        /// <summary>
        /// Add Task In Team
        /// </summary>
        /// <param name="task">Task Name, Team Id and User Id</param>
        /// <returns>True - If successfully Add Task alse False</returns>
        public bool AddTask(TaskDetailViewModel task)
        {
            if (task != null)
            {
                if (task.StartDate == null && task.EndDate == null)
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
        /// Get All Notifications
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all notifications</returns>
        public List<Notification> GetNotifications(long userId)
        {
            var allNotifications = _db.Notifications.Where(notification => notification.UserId == userId && notification.IsDeleted == false);

            List<Notification> notifications = new();

            foreach (var notificationRow in allNotifications)
            {
                Notification notification = new();
                notification.NotificationId = notificationRow.NotificationId;
                notification.NotificationType = notificationRow.Type.ToString();
                notification.Message = notificationRow.Message;
                notification.IsRead = notificationRow.IsRead;

                notifications.Add(notification);
            }

            return notifications;
        }

        /// <summary>
        /// Clear All Notification
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If all notifications cleared else False</returns>
        public bool ClearAllNotifications(long userId)
        {
            var notifications = _db.Notifications.Where(notification => notification.UserId == userId);
            if (notifications.Any())
            {
                foreach (var notification in notifications)
                {
                    notification.IsDeleted = true;

                    _db.Update(notification);
                }
                _db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}

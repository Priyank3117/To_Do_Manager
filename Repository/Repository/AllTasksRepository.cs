﻿using Entities.Data;
using Entities.Models;
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

        /// <summary>
        /// Get All Task In All Task Page Team Wise
        /// </summary>
        /// <param name="filter">filter params like Team Name, Task Name, Start Date, End Date and Task Status</param>
        /// <returns>List of All Task Team wise</returns>
        public List<AllTasksOfAllTeams> GetAllTasks(Filter filter)
        {
            List<AllTasksOfAllTeams> teams = new();

            var query = _db.Tasks.AsQueryable();
            var teamMemberQuery = _db.TeamMembers.AsQueryable();

            if (!string.IsNullOrEmpty(filter.TeamName))
            {
                query = query.Where(task => task.Teams.TeamName.ToLower().Contains(filter.TeamName.ToLower()));
            }

            if (!string.IsNullOrEmpty(filter.TaskName))
            {
                query = query.Where(task => task.TaskName.ToLower().Contains(filter.TaskName.ToLower()));
            }

            if (filter.StartDate != null)
            {
                query = query.Where(task => task.StartDate.Date >= filter.StartDate.Value.Date);
            }

            if (filter.EndDate != null)
            {
                query = query.Where(task => task.EndDate.Date <= filter.EndDate.Value.Date);
            }

            if (filter.TaskStatus != null)
            {
                query = query.Where(task => task.TaskStatus == filter.TaskStatus);
            }

            var totalTeams = teamMemberQuery.Where(teamMembers => teamMembers.UserId == filter.UserId && (teamMembers.Status == TeamMembers.MemberStatus.Approved || teamMembers.Status == TeamMembers.MemberStatus.RequestedForLeave)).Select(teamMember => teamMember.TeamId).ToList();

            foreach (var teamId in totalTeams)
            {
                AllTasksOfAllTeams team = new();
                var myTasks = query.Where(task => task.UserId == filter.UserId && task.TeamId == teamId);
                team.TeamId = teamId;
                team.TeamName = query.Where(p => p.Teams.TeamId == teamId).Select(p => p.Teams.TeamName).FirstOrDefault()!;
                team.UserId = filter.UserId;
                team.Role = teamMemberQuery.FirstOrDefault(teamMember => teamMember.UserId == filter.UserId && teamMember.TeamId == teamId)!.Role.ToString();

                foreach (var mytask in myTasks)
                {
                    TaskDetailViewModel taskDetailViewModel = new();
                    taskDetailViewModel.TaskName = mytask.TaskName;
                    taskDetailViewModel.IsCompleted = mytask.TaskStatus;
                    taskDetailViewModel.TaskId = mytask.TaskId;
                    taskDetailViewModel.UserId = filter.UserId;
                    taskDetailViewModel.TeamId = teamId;
                    taskDetailViewModel.IsTodayTask = mytask.StartDate.Date <= DateTime.Now.Date && mytask.EndDate >= DateTime.Now.Date && mytask.IsTaskForToday == true ? true : false;
                    taskDetailViewModel.StartDateForDisplay = mytask.StartDate.Date.ToString("dd/MM/yyyy");
                    taskDetailViewModel.EndDateForDisplay = mytask.EndDate.Date.ToString("dd/MM/yyyy");

                    team.MyTasks.Add(taskDetailViewModel);
                }

                List<long> totalMembers = new();

                if (team.Role == "TeamLeader")
                {
                    totalMembers = query.Where(task => task.UserId != filter.UserId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();
                }
                else if (team.Role == "ReportingPerson")
                {
                    totalMembers = teamMemberQuery.Where(task => task.UserId != filter.UserId && task.TeamId == teamId && task.ReportinPersonUserId == filter.UserId).Select(p => p.UserId).Distinct().ToList();
                }

                foreach (var memberTask in totalMembers)
                {
                    var teamMemberTasks = query.Where(task => task.UserId == memberTask && task.TeamId == teamId);

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
                            taskDetailViewModel.IsTodayTask = task.StartDate.Date <= DateTime.Now.Date && task.EndDate.Date >= DateTime.Now.Date && task.IsTaskForToday == true ? true : false;
                            taskDetailViewModel.StartDateForDisplay = task.StartDate.Date.ToString("dd/MM/yyyy");
                            taskDetailViewModel.EndDateForDisplay = task.EndDate.Date.ToString("dd/MM/yyyy");

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
        /// Get Data For Add Task To Today Task
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>List of Users details like Avatar, FirstName, LastName, and User Id</returns>
        public List<ListOfUsers> GetForAddTaskToToDo(long teamId, long userId)
        {
            var query = _db.TeamMembers.AsQueryable();

            var memberRole = query.FirstOrDefault(teamMember => teamMember.TeamId == teamId && teamMember.UserId == userId)!.Role;

            if (memberRole == TeamMembers.Roles.TeamLeader)
            {
                return query.Where(teamMember => teamMember.TeamId == teamId).Select(task => new ListOfUsers()
                {
                    UserId = task.UserId,
                    UserName = task.Users.FirstName + " " + task.Users.LastName,
                    Avatar = task.Users.Avatar
                }).ToList();
            }
            else if (memberRole == TeamMembers.Roles.ReportingPerson)
            {
                return query.Where(teamMember => teamMember.TeamId == teamId && teamMember.ReportinPersonUserId == userId).Select(p => new ListOfUsers()
                {
                    UserName = p.Users.FirstName + " " + p.Users.LastName,
                    UserId = p.UserId,
                    Avatar = p.Users.Avatar
                }).ToList();
            }
            else
            {
                return new List<ListOfUsers>();
            }
        }

        /// <summary>
        /// Add Task To To-Do Page
        /// </summary>
        /// <param name="taskDetail">Teak Detail View Model for storing Task Id, Team Id and User Id</param>
        /// <returns>True - If Successfully Added alse False</returns>
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

        /// <summary>
        /// Add Task To Today Task For TeamMember
        /// </summary>
        /// <param name="taskDetail">TaskDetails like User Id of that task and User Id of who task assigned</param>
        /// <returns>True - If task successfully added to Today task else False</returns>
        public bool AddTaskToTodayTaskForTeamMember(TaskDetailViewModel taskDetail)
        {
            var fromUser = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == taskDetail.FromUserId && teamMember.TeamId == taskDetail.TeamId)!.Role;

            Tasks.AssignBy assignBy = new();

            if (fromUser == TeamMembers.Roles.ReportingPerson)
            {
                assignBy = Tasks.AssignBy.ReportingPerson;
            }
            else if (fromUser == TeamMembers.Roles.TeamLeader)
            {
                assignBy = Tasks.AssignBy.TeamLeader;
            }

            var task = _db.Tasks.FirstOrDefault(task => task.TaskId == taskDetail.TaskId)!;

            if (task != null)
            {
                task.UserId = taskDetail.UserId;
                task.IsTaskForToday = true;
                task.StartDate = DateTime.Now;
                task.EndDate = DateTime.Now;
                task.AssignedBy = assignBy;

                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get All Task Name for Calender View
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all task for that user with task details like
        /// Start Date, End Date and Task Status whether it is completed or not</returns>
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
                    task.EndDate = mytask.EndDate.AddDays(1).ToString("yyyy-MM-dd");
                    task.TaskDescription = mytask.TaskDescription;
                    task.TaskId = mytask.TaskId;

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
                                taskOfMember.EndDate = task.EndDate.AddDays(1).ToString("yyyy-MM-dd");
                                taskOfMember.TaskDescription = task.TaskDescription;
                                taskOfMember.TaskId = task.TaskId;

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

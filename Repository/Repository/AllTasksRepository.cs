﻿using Entities.Data;
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

            query = query.OrderByDescending(task => task.IsTodayTask);

            var totalTeams = query.Where(task => task.UserId == userId).Select(task => task.TeamId).Distinct().ToList();

            foreach (var teamId in totalTeams)
            {
                AllTasksOfAllTeams team = new();
                var myTasks = query.Where(task => task.UserId == userId && task.TeamId == teamId);
                team.TeamId = teamId;
                team.TeamName = query.Select(p => p.Teams.TeamName).FirstOrDefault()!;
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
                    taskDetailViewModel.IsTodayTask = mytask.IsTodayTask;

                    team.MyTasks.Add(taskDetailViewModel);
                }

                if (team.Role != "TeamMember")
                {
                    var totalMembers = query.Where(task => task.UserId != userId && task.TeamId == teamId).Select(p => p.UserId).Distinct().ToList();

                    foreach (var memberTask in totalMembers)
                    {
                        var teamMemberTasks = query.Where(task => task.UserId == memberTask && task.TeamId == teamId);

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
                            taskDetailViewModel.IsTodayTask = task.IsTodayTask;

                            teamMembersTaskDetails.TodayTasks.Add(taskDetailViewModel);
                        }
                        team.TeamMembersTaasks.Add(teamMembersTaskDetails);
                    }
                }

                teams.Add(team);
            }

            return teams;
        }

        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail)
        {
            var task = _db.Tasks.FirstOrDefault(task => task.TaskId == taskDetail.TaskId && task.TeamId == taskDetail.TeamId && task.UserId == taskDetail.UserId)!;

            if(task.IsTodayTask == null)
            {
                task.IsTodayTask = DateTime.Now;
                _db.Update(task);
                _db.SaveChanges();

                return true;
            }
            else
            {
                task.IsTodayTask = null;
                _db.Update(task);
                _db.SaveChanges();

                return false;
            }
        }
    }
}

﻿using Entities;
using Entities.ViewModels.HomeViewModels;
using Repository.Interface;
using System.Net;
using System.Net.Mail;

namespace BAL
{
    public class HomeBAL
    {
        private readonly IHomeRepository _HomeRepo;
        private readonly INotificationRepository _NotificationRepo;
        private readonly MailHelper mailHelper;

        public HomeBAL(IHomeRepository homeRepo, INotificationRepository notificationRepo, MailHelper mail)
        {
            _HomeRepo = homeRepo;
            _NotificationRepo = notificationRepo;
            mailHelper = mail;
        }
        
        /// <summary>
        /// Get Available All Teams with Team Name
        /// </summary>
        /// <param name="searchTerm">Search Keyword</param>
        /// <param name="userId"></param>
        /// <returns>List of all available teams</returns>
        public List<AllTeamsViewModel> GetAllTeams(string searchTerm, long userId)
        {
            return _HomeRepo.GetAllTeams(searchTerm, userId);
        }

        /// <summary>
        /// Get Teams Details
        /// </summary>
        /// <param name="teamId">TeamId</param>
        /// <returns>All Tasks with Team Name</returns>
        public TeamViewModel GetTeamDetails(long teamId)
        {
            return _HomeRepo.GetTeamDetails(teamId);
        }

        /// <summary>
        /// Create Team
        /// </summary>
        /// <param name="team">Team Details like Team Name, Team Discription and Team Members</param>
        /// <returns>True - If successfully team created else False</returns>
        public bool CreateTeam(TeamViewModel team)
        {
            var teamId = _HomeRepo.CreateTeam(team);
            if (teamId != 0)
            {
                foreach (var userEmail in team.UserEmails)
                {
                    if (!_HomeRepo.AddUserToTeam(userEmail, teamId))
                    {
                        var button = "<center><a role=\"button\" style=\" background-color: gray; color: black: border-radius: 10px; \" href=https://localhost:7100/Account/Registration >Join Team</a></center>";

                        string body = "<div>You have an invitation to join <b> " + team.TeamName + " </ b > Team click below button to join team \n\n" + button + "</div>";

                        if (team.MessageForMembers != null)
                        {
                            body += "\n <h4> You have one special message from Team Leader:</h4>\n\n <b>" + team.MessageForMembers + "</b>";
                        }

                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "<div>" + body + "</div>",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };

                        mailHelper.SendEmail(sendEmailViewModel);
                    }
                    else
                    {
                        var button = "<center><a role=\"button\" style=\" background-color: gray; color: black: border-radius: 10px; \" href=https://localhost:7100 >Join Team</a></center>";

                        string body = "<div>You have an invitation to join <b> " + team.TeamName + " </ b > Team click below button to join team \n\n" + button + "</div>";

                        if (team.MessageForMembers != null)
                        {
                            body += "\n <h4> You have one special message from Team Leader:</h4>\n\n <b>" + team.MessageForMembers + "</b>";
                        }

                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "<div>" + body + "</div>",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };

                        mailHelper.SendEmail(sendEmailViewModel);
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Request to Join Team
        /// </summary>
        /// <param name="userRequest">UserId, TeamId</param>
        /// <returns>True - If successfully requested alse False</returns>
        public bool RequestToJoinTeam(TeamMemberViewModel userRequest)
        {
            return _HomeRepo.RequestToJoinTeam(userRequest);
        }

        /// <summary>
        /// Get All Today's Task for To-Do List Page
        /// </summary>
        /// <param name="userId">User Id of user</param>
        /// <returns>List of all today's task of this user with task details like task status whether it is completed or not and task name</returns>
        public List<TodayTasksViewModel> GetAllTodayTasks(long userId)
        {
            return _HomeRepo.GetAllTodayTasks(userId);
        }
        
        public List<AllTodayTasksForListView> GetAllTodayTasksForListView(long userId)
        {
            return _HomeRepo.GetAllTodayTasksForListView(userId);
        }

        /// <summary>
        /// Get Data for Add Task
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>List of User's UserName with User Id</returns>
        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            return _HomeRepo.GetDataForAddTask(teamId, userId);
        }

        /// <summary>
        /// Add Task In Team
        /// </summary>
        /// <param name="task">Task Name, Team Id and User Id</param>
        /// <returns>True - If successfully Add Task alse False</returns>
        public bool AddTask(TaskDetailViewModel task)
        {
            return _HomeRepo.AddTask(task);
        }

        /// <summary>
        /// Mark Task as Complete Or Uncomplete
        /// </summary>
        /// <param name="task">Task Id, Team Id and User Id</param>
        /// <returns>True - If successful alse False</returns>
        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel task)
        {
            return _HomeRepo.MarkTaskAsCompleteOrUncomplete(task);
        }

        /// <summary>
        /// Get Task Details
        /// </summary>
        /// <param name="taskId">Task Id</param>
        /// <returns>Task Details like - Task Name, Task Description, Start Date, End Date and IsCompleted or Not</returns>
        public TaskDetailViewModel GetTaskDetails(long taskId)
        {
            return _HomeRepo.GetTaskDetails(taskId);
        }

        /// <summary>
        /// Get All Notifications
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all notifications</returns>
        public List<Notification> GetNotifications(long userId)
        {
            return _NotificationRepo.GetNotifications(userId);
        }

        /// <summary>
        /// Clear All Notification
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If all notifications cleared else False</returns>
        public bool ClearAllNotifications(long userId)
        {
            return _NotificationRepo.ClearAllNotifications(userId);
        }

        /// <summary>
        /// Mark Notification as Read
        /// </summary>
        /// <param name="notificationId">Notification Id</param>
        /// <returns>True - If Notification successfully mark as read else False</returns>
        public bool MarkNotificationAsRead(long notificationId)
        {
            return _NotificationRepo.MarkNotificationAsRead(notificationId);
        }

        /// <summary>
        /// Edit Task In Task Description OffCanvas
        /// </summary>
        /// <param name="task">Task Details like - Task Id, Task Name, Task Description, Start Date, End Date and Task Status</param>
        /// <returns>True - If task successfully edited else False</returns>
        public bool EditTask(TaskDetailViewModel task)
        {
            return _HomeRepo.EditTask(task);
        }
    }
}

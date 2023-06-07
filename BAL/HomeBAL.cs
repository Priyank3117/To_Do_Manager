using Entities.ViewModels.HomeViewModels;
using Repository.Interface;
using System.Net;
using System.Net.Mail;

namespace BAL
{
    public class HomeBAL
    {
        private readonly IHomeRepository _HomeRepo;

        public HomeBAL(IHomeRepository homeRepo)
        {
            _HomeRepo = homeRepo;
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
                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "You have an invitation to join <b>" + team.TeamName + "</b> Team click below link to see https://localhost:7100/Account/Registration",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };
                        InviteUser(sendEmailViewModel);
                    }
                    else
                    {
                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "You have an invitation to join <b>" + team.TeamName + "</b> Team click below link to see https://localhost:7100",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };
                        InviteUser(sendEmailViewModel);
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
        /// Invite User Via Email
        /// </summary>
        /// <param name="emailInfo">Email Subject, Email Body and Recevier's Email Id</param>
        /// <returns>True - Id successfully email sent to user else False</returns>
        public bool InviteUser(SendEmailViewModel emailInfo)
        {
            var fromEmail = new MailAddress("chavdaanand2002@gmail.com");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, "tomkciepchiqkbkx")
            };

            MailMessage message = new(fromEmail, new MailAddress(emailInfo.ToEmail))
            {
                Subject = emailInfo.Subject,
                Body = emailInfo.Body,
                IsBodyHtml = true
            };
            smtp.Send(message);

            return true;
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
            return _HomeRepo.GetNotifications(userId);
        }

        /// <summary>
        /// Clear All Notification
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If all notifications cleared else False</returns>
        public bool ClearAllNotifications(long userId)
        {
            return _HomeRepo.ClearAllNotifications(userId);
        }
    }
}

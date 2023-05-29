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

        public List<AllTeamsViewModel> GetAllTeams(string searchTerm, long userId)
        {
            return _HomeRepo.GetAllTeams(searchTerm, userId);
        }

        public TeamViewModel GetTeamDetails(long teamId)
        {
            return _HomeRepo.GetTeamDetails(teamId);
        }

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
                            Body = "You have an invitation to join <b>" + team.TeamName + "</b> Team",
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

        public bool RequestToJoinTeam(TeamMemberViewModel userRequest)
        {
            return _HomeRepo.RequestToJoinTeam(userRequest);
        }

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

        public List<TodayTasksViewModel> GetAllTodayTasks(long userId)
        {
            return _HomeRepo.GetAllTodayTasks(userId);
        }

        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId)
        {
            return _HomeRepo.GetDataForAddTask(teamId, userId);
        }

        public bool AddTask(TaskDetailViewModel task)
        {
            return _HomeRepo.AddTask(task);
        }

        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel task)
        {
            return _HomeRepo.MarkTaskAsCompleteOrUncomplete(task);
        }
    }
}

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

        public bool CreateTeam(TeamViewModel team)
        {
            var teamId = _HomeRepo.CreateTeam(team);
            if (teamId != 0)
            {
                foreach (var user in team.UserEmails)
                {
                    if (!_HomeRepo.AddUserToTeam(user, teamId))
                    {
                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "You have an invitation to join <b>" + team.TeamName + "</b> Team",
                            Subject = "Invitation to join a Team",
                            ToEmail = user
                        };
                        InviteUser(sendEmailViewModel);
                    }
                }
                return true;
            }
            return false;
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
    }
}

using Entities.ViewModels.HomeViewModels;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Entities
{
    public class MailHelper
    {
        private readonly IConfiguration _configuration;

        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="sendEmail">Email Subject, Email Body and Recevier's Email Id</param>
        /// <returns>True - If successfully email sent to user else False</returns>
        public bool SendEmail(SendEmailViewModel sendEmail)
        {
            if(sendEmail != null)
            {
                var fromEmailAddress = _configuration.GetSection("EmailSettings").GetSection("FromEmail").Value;

                var fromEmail = new MailAddress(fromEmailAddress!);          

                var smtp = new SmtpClient
                {
                    Host = _configuration.GetSection("EmailSettings").GetSection("Host").Value!,
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, _configuration.GetSection("EmailSettings").GetSection("EmailPassword").Value)
                };

                MailMessage message = new(fromEmail, new MailAddress(sendEmail.ToEmail))
                {
                    Subject = sendEmail.Subject,
                    Body = sendEmail.Body,
                    IsBodyHtml = true
                };
                smtp.Send(message);

                return true;
            }
            else
            {
                return false;
            }            
        }
    }
}

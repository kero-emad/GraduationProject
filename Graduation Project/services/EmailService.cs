using System.Net.Mail;
using System.Net;

namespace Graduation_Project.services
{
    public class EmailService:IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly string _smtpUsername = "eduplaysystem@gmail.com";
        private readonly string _smtpPassword = "jypqyjpvpktgalnd";


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var client = new SmtpClient(_smtpServer)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUsername),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
            catch(SmtpException ex) {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new Exception("could not send email try again later");
            }
        }
    }
}

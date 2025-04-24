using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace esii.stratagies.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["Mailtrap:Host"])
            {
                Port = int.Parse(_configuration["Mailtrap:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["Mailtrap:Username"],
                    _configuration["Mailtrap:Password"]
                ),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("noreply@teusite.com", "ESII App"),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
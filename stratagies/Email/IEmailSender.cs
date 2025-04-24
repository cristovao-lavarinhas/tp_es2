namespace esii.stratagies.Email;

public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string body);
}
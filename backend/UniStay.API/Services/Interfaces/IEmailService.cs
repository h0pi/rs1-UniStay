public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
    Task SendPasswordResetTokensAsync(string toEmail, string resetToken);

}
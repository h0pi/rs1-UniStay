using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtp = new SmtpClient(_config["EmailSettings:SmtpHost"])
        {
            Port = int.Parse(_config["EmailSettings:SmtpPort"]),
            EnableSsl = true,
            UseDefaultCredentials = false, // 🔥 OVO JE NEDOSTAJALO!
            Credentials = new NetworkCredential(
                _config["EmailSettings:SmtpUser"],
                _config["EmailSettings:SmtpPass"]
            )
        };

        var mail = new MailMessage
        {
            From = new MailAddress(_config["EmailSettings:SmtpUser"], "UniStay Notifications"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(toEmail);

        await smtp.SendMailAsync(mail);
    }

    public async Task SendPasswordResetTokensAsync(string toEmail, string resetToken)
    {
        string url = $"http://localhost:4200/reset-password/{resetToken}";

        string body = $@"
            <h2>Password Reset</h2>
            <p>Use the token below to reset your password:</p>
            <p><b>{resetToken}</b></p>
            <p>Or click the link:</p>
            <a href='{url}'>Reset Password</a>
        ";

        await SendEmailAsync(toEmail, "Password Reset Request", body);
    }
}
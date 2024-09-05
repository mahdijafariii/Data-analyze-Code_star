using System.Net;
using System.Net.Mail;

namespace AnalysisData.User.Services.EmailService;

public class EmailService : IEmailService
{
    
    
    public async Task SendPasswordResetEmail(string toEmail, string resetLink)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress("asd"),
            Subject = "Password Reset Request",
            Body = $"To reset your password, please click the following link: {resetLink}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        using var smtpClient = new SmtpClient("asd", 564)
        {
            Credentials = new NetworkCredential("asdas", "asd"),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(mailMessage);
    }
}
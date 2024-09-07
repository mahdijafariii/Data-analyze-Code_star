using System.Net;
using System.Net.Mail;
using AnalysisData.Exception;

namespace AnalysisData.User.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;
    private readonly string _fromEmail;

    public EmailService(IConfiguration configuration)
    {
        _smtpServer = configuration["EmailSettings:SmtpServer"];
        _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
        _smtpUser = configuration["EmailSettings:SmtpUser"];
        _smtpPass = configuration["EmailSettings:SmtpPass"];
        _fromEmail = configuration["EmailSettings:FromEmail"];
    }

    public async Task SendPasswordResetEmail(string toEmail, string resetLink)
    {
        string htmlTemplatePath = @"Assets\email-template.html";
        string htmlContent = File.ReadAllText(htmlTemplatePath);
        htmlContent = htmlContent.Replace("{resetLink}", resetLink);

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = "Password Reset Request",
            Body = $"To reset your password, please click the following link: {resetLink}",
            IsBodyHtml = true,
        };

        mailMessage.To.Add(toEmail);

        using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
        {
            Credentials = new NetworkCredential(_smtpUser, _smtpPass),
            EnableSsl = true
        };

        try
        {
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlContent, null, "text/html");
            mailMessage.AlternateViews.Add(htmlView);
            await smtpClient.SendMailAsync(mailMessage);
        }
        catch (SmtpException ex)
        {
            Console.WriteLine($"SMTP Exception: {ex.Message}");
            throw new OperationFailedException();
        }
    }
}
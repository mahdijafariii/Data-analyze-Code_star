namespace AnalysisData.User.Services.EmailService;

public interface IEmailService
{
    Task SendPasswordResetEmail(string toEmail, string resetLink);
}
namespace AnalysisData.Services.EmailService.Abstraction;

public interface IEmailService
{
    Task SendPasswordResetEmail(string toEmail, string resetLink);
}
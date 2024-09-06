namespace AnalysisData.User.Services.TokenService.Abstraction;

public interface IValidateTokenService
{
    Task ValidateResetToken(Guid userId, string resetPasswordToken);
}
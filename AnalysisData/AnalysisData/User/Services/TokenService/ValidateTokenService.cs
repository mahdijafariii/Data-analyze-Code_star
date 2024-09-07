using AnalysisData.Exception.TokenException;
using AnalysisData.User.Repository.PasswordResetTokensRepository.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;

public class ValidateTokenService : IValidateTokenService
{
    private readonly IPasswordResetTokensRepository _resetTokensRepository;

    public ValidateTokenService(IPasswordResetTokensRepository resetTokensRepository)
    {
        _resetTokensRepository = resetTokensRepository;
    }

    public async Task ValidateResetToken(Guid userId, string resetPasswordToken)
    {
        var resetToken = await _resetTokensRepository.GetToken(userId);
        if (resetToken == null || resetToken.IsUsed)
            throw new TokenIsInvalidException();
        if (resetPasswordToken != resetToken.Token)
            throw new TokenIsInvalidException();
        if (resetToken.Expiration < DateTime.UtcNow)
            throw new TokenExpiredException();

        resetToken.IsUsed = true;
        await _resetTokensRepository.SaveChange();
    }
}
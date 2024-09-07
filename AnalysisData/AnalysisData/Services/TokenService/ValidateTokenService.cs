using AnalysisData.Exception.TokenException;
using AnalysisData.Repositories.PasswordResetTokensRepository.Abstraction;
using AnalysisData.Services.TokenService.Abstraction;

namespace AnalysisData.Services.TokenService;

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
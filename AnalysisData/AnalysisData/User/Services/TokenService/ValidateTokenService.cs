using AnalysisData.User.Repository.PasswordResetTokensRepository.Abstraction;
using AnalysisData.User.Services.TokenService.Abstraction;

public class ValidateTokenService : IValidateTokenService
{
    private readonly IPasswordResetTokensRepository _resetTokensRepository;

    public ValidateTokenService(IPasswordResetTokensRepository resetTokensRepository)
    {
        _resetTokensRepository = resetTokensRepository;
    }

    public async Task ValidateResetToken(Guid userId)
    {
        var resetToken = await _resetTokensRepository.GetToken(userId);
        if (resetToken == null || resetToken.IsUsed)
            throw new Exception("Invalid token or token already used.");

        if (resetToken.Expiration < DateTime.UtcNow)
            throw new Exception("Token has expired.");

        resetToken.IsUsed = true;
        await _resetTokensRepository.SaveChange();
    }
}
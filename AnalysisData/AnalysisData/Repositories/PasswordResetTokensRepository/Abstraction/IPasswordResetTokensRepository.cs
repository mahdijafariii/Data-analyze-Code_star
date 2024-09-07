using AnalysisData.Models.UserModel;

namespace AnalysisData.Repositories.PasswordResetTokensRepository.Abstraction;

public interface IPasswordResetTokensRepository
{
    Task AddToken(PasswordResetToken token);
    Task<PasswordResetToken> GetToken(Guid guid);
    Task SaveChange();

}
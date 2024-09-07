using AnalysisData.User.Model;

namespace AnalysisData.User.Repository.PasswordResetTokensRepository.Abstraction;

public interface IPasswordResetTokensRepository
{
    Task AddToken(PasswordResetToken token);
    Task<PasswordResetToken> GetToken(Guid guid);
    Task SaveChange();

}
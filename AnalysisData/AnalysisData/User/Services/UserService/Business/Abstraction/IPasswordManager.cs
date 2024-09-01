using AnalysisData.UserManage.Model;

namespace AnalysisData.Services.Business.Abstraction;

public interface IPasswordManager
{
    Task<bool> ResetPasswordAsync(User user, string password, string confirmPassword);
    Task<bool> NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword);
}
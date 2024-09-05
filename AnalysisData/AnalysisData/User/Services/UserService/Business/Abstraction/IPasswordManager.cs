using AnalysisData.UserManage.Model;

namespace AnalysisData.Services.Business.Abstraction;

public interface IPasswordManager
{
    Task ResetPasswordAsync(User user, string password, string confirmPassword);
    Task NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword);
}
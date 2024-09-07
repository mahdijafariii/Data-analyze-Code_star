
using AnalysisData.Models.UserModel;

namespace AnalysisData.Services.UserService.UserService.Abstraction;

public interface IPasswordService
{
    Task ResetPasswordAsync(User user, string password, string confirmPassword,
        string resetPasswordToken);
    Task NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword);
}
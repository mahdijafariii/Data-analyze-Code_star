
using AnalysisData.User.Model;

public interface IPasswordManager
{
    Task ResetPasswordAsync(User user, string password, string confirmPassword,
        string resetPasswordToken);
    Task NewPasswordAsync(User user, string oldPassword, string password, string confirmPassword);
}
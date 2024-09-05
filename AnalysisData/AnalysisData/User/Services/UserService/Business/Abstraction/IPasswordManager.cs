namespace AnalysisData.User.Services.UserService.Business.Abstraction;

public interface IPasswordManager
{
    Task ResetPasswordAsync(Model.User user, string password, string confirmPassword);
    Task NewPasswordAsync(Model.User user, string oldPassword, string password, string confirmPassword);
}
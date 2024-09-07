

using AnalysisData.Models.UserModel;

namespace AnalysisData.Services.UserService.UserService.Business.Abstraction;

public interface IValidtionPasswordManager
{
    public void ValidatePassword(User user, string password);
    public void ValidatePasswordAndConfirmation(string password, string confirmPassword);
    public string HashPassword(string password);
}
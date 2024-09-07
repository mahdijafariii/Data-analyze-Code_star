
namespace AnalysisData.User.Services.UserService.Business.Abstraction;

public interface IPasswordService
{
    public void ValidatePassword(Model.User user, string password);
    public void ValidatePasswordAndConfirmation(string password, string confirmPassword);
    public string HashPassword(string password);
}
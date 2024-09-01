using AnalysisData.UserManage.Model;

namespace AnalysisData.Services.Business.Abstraction;

public interface IPasswordService
{
    public void ValidatePassword(User user, string password);
    public void ValidatePasswordAndConfirmation(string password, string confirmPassword);
    public string HashPassword(string password);
}
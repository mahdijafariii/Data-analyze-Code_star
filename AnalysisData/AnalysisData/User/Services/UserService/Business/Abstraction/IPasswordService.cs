

using AnalysisData.User.Model;

public interface IPasswordService
{
    public void ValidatePassword(User user, string password);
    public void ValidatePasswordAndConfirmation(string password, string confirmPassword);
    public string HashPassword(string password);
}
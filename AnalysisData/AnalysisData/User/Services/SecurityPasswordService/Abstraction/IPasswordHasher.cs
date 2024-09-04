namespace AnalysisData.User.Services.SecurityPasswordService.Abstraction;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
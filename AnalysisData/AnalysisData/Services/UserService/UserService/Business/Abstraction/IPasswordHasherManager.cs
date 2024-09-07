namespace AnalysisData.Services.UserService.UserService.Business.Abstraction;

public interface IPasswordHasherManager
{
    string HashPassword(string password);
}
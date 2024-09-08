using AnalysisData.Models.UserModel;

namespace AnalysisData.Services.JwtService.Abstraction;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string userName);
    Task UpdateUserCookie(string userName, bool rememberMe);
    Task<string> RequestResetPassword(User user);
}
namespace AnalysisData.JwtService.abstractions;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string userName);
    Task UpdateUserCookie(string userName, bool rememberMe);
}
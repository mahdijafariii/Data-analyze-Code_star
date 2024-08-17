namespace AnalysisData.JwtService.abstractions;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string userName);
}
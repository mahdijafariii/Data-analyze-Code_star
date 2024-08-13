namespace AnalysisData.JwtService;

public interface IJwtService
{
    Task<string> GenerateJwtToken(string userName);
}
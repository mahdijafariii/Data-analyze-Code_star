using AnalysisData.CookieService.abstractions;
using AnalysisData.JwtService.abstractions;

namespace AnalysisData.Services;

public class UpdateJwtService
{
    private readonly IJwtService _jwtService;
    private readonly ICookieService _cookieService;

    public UpdateJwtService(IJwtService jwtService, ICookieService cookieService)
    {
        _jwtService = jwtService;
        _cookieService = cookieService;
    }
}
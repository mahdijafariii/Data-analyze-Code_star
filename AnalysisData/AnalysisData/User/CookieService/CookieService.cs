using AnalysisData.User.CookieService.abstractions;

namespace AnalysisData.User.CookieService;

public class CookieService : ICookieService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetCookie(string name, string token, bool rememberMe)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        };

        if (rememberMe)
        {
            options.Expires = DateTimeOffset.UtcNow.AddDays(7);
        }
        else
        {
            options.IsEssential = true;
        }

        _httpContextAccessor.HttpContext.Response.Cookies.Append(name, token, options);
    }

    public string GetCookie(string name)
    {
        _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(name, out var value);
        return value;
    }

    public void RemoveCookie(string name)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Delete(name);
    }

    public void UpdateCookie(string name, string newToken, bool rememberMe)
    {
        RemoveCookie(name);
        SetCookie(name, newToken, rememberMe);
    }
}
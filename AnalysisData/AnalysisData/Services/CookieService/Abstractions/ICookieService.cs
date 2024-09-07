namespace AnalysisData.Services.CookieService.Abstractions;

public interface ICookieService
{
    void RemoveCookie(string name);
    string GetCookie(string name);
    void SetCookie(string name, string token, bool rememberMe);
    public void UpdateCookie(string name, string newToken, bool rememberMe);
}
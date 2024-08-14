namespace AnalysisData.CookieService.abstractions;

public interface ICookieService
{
    void RemoveCookie(string name);
    string GetCookie(string name);
    void SetCookie(string name, string token,bool rememberMe);
}
namespace AnalysisData.Exception;

public class TokenNotFoundInCookieException : System.Exception
{
    public TokenNotFoundInCookieException() : base(Resources.TokenNotFoundInCookieException)
    {
    }
}
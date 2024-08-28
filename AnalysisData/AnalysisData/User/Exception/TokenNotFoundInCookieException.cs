namespace AnalysisData.Exception;

public class TokenNotFoundInCookieException : ServiceException
{
    public TokenNotFoundInCookieException() : base(Resources.TokenNotFoundInCookieException,StatusCodes.Status404NotFound)
    {
    }
}
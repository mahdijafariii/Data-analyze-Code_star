namespace AnalysisData.Exception.TokenException;

public class TokenNotFoundInCookieException : ServiceException
{
    public TokenNotFoundInCookieException() : base(Resources.TokenNotFoundInCookieException,
        StatusCodes.Status404NotFound)
    {
    }
}
namespace AnalysisData.Exception.UserException;

public class TokenNotFoundInCookieException : ServiceException
{
    public TokenNotFoundInCookieException() : base(Resources.TokenNotFoundInCookieException,
        StatusCodes.Status404NotFound)
    {
    }
}
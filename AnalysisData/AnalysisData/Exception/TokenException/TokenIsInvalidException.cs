namespace AnalysisData.Exception.TokenException;

public class TokenIsInvalidException : ServiceException
{
    public TokenIsInvalidException() : base(Resources.TokenIsInvalidException, StatusCodes.Status400BadRequest)
    {
    }
}
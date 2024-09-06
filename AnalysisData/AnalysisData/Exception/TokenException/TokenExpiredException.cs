namespace AnalysisData.Exception.TokenException;

public class TokenExpiredException : ServiceException
{
    public TokenExpiredException() : base(Resources.TokenExpiredException, StatusCodes.Status400BadRequest)
    {
    }
}
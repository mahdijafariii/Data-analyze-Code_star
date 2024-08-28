namespace AnalysisData.Exception;

public class InvalidPasswordFormatException : ServiceException
{
    public InvalidPasswordFormatException() : base(Resources.InvalidPasswordFormatException, StatusCodes.Status401Unauthorized)
    {
    }
}
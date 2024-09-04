namespace AnalysisData.Exception.UserException;

public class InvalidPasswordFormatException : ServiceException
{
    public InvalidPasswordFormatException() : base(Resources.InvalidPasswordFormatException,
        StatusCodes.Status400BadRequest)
    {
    }
}
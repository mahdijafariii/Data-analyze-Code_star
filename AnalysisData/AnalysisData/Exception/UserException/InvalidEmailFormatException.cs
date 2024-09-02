namespace AnalysisData.Exception.UserException;

public class InvalidEmailFormatException : ServiceException
{
    public InvalidEmailFormatException() : base(Resources.InvalidEmailFormatException, StatusCodes.Status400BadRequest)
    {
    }
}
namespace AnalysisData.Exception.InvalidFormatException;

public class InvalidEmailFormatException : ServiceException
{
    public InvalidEmailFormatException() : base(Resources.InvalidEmailFormatException, StatusCodes.Status400BadRequest)
    {
    }
}
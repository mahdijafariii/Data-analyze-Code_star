namespace AnalysisData.Exception.UserException;

public class InvalidPasswordException : ServiceException
{
    public InvalidPasswordException() : base(Resources.InvalidPasswordException, StatusCodes.Status400BadRequest)
    {
    }
}
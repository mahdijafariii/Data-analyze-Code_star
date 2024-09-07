namespace AnalysisData.Exception.PasswordException;

public class InvalidPasswordException : ServiceException
{
    public InvalidPasswordException() : base(Resources.InvalidPasswordException, StatusCodes.Status400BadRequest)
    {
    }
}
namespace AnalysisData.Exception.PasswordException;

public class InvalidPasswordFormatException : ServiceException
{
    public InvalidPasswordFormatException() : base(Resources.InvalidPasswordFormatException,
        StatusCodes.Status400BadRequest)
    {
    }
}
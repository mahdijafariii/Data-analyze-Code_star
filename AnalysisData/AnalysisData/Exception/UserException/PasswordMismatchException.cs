namespace AnalysisData.Exception.UserException;

public class PasswordMismatchException : ServiceException
{
    public PasswordMismatchException() : base(Resources.PasswordMismatchException, StatusCodes.Status400BadRequest)
    {
    }
}
namespace AnalysisData.Exception;

public class PasswordMismatchException : ServiceException
{
    public PasswordMismatchException() : base(Resources.PasswordMismatchException,StatusCodes.Status401Unauthorized )
    {
    }
}
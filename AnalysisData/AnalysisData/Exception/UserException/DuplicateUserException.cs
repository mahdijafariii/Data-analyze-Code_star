namespace AnalysisData.Exception.UserException;

public class DuplicateUserException : ServiceException
{
    public DuplicateUserException() : base(Resources.DuplicateUserException, StatusCodes.Status400BadRequest)
    {
    }
}
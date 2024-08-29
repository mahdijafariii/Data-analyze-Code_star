namespace AnalysisData.Exception;

public class DuplicateUserException : ServiceException
{
    public DuplicateUserException() : base(Resources.DuplicateUserException, StatusCodes.Status400BadRequest)
    {
    }
}
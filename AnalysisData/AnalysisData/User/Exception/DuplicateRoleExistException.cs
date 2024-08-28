namespace AnalysisData.Exception;

public class DuplicateRoleExistException : ServiceException
{
    public DuplicateRoleExistException() : base(Resources.DuplicateRoleException,StatusCodes.Status403Forbidden)
    {
    }
}
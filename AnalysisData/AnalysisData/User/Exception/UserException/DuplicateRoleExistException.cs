namespace AnalysisData.Exception;

public class DuplicateRoleExistException : ServiceException
{
    public DuplicateRoleExistException() : base(Resources.DuplicateRoleException,StatusCodes.Status400BadRequest)
    {
    }
}
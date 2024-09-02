namespace AnalysisData.Exception.UserException;

public class DuplicateRoleExistException : ServiceException
{
    public DuplicateRoleExistException() : base(Resources.DuplicateRoleException, StatusCodes.Status400BadRequest)
    {
    }
}
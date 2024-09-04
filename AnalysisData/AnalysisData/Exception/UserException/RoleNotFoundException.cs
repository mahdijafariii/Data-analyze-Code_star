namespace AnalysisData.Exception.UserException;

public class RoleNotFoundException : ServiceException
{
    public RoleNotFoundException() : base(Resources.RoleNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
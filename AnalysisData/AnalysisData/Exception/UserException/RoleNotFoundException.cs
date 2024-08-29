namespace AnalysisData.Exception;

public class RoleNotFoundException : ServiceException
{
    public RoleNotFoundException() : base(Resources.RoleNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
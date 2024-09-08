namespace AnalysisData.Exception.UserException;

public class AdminProtectedException : ServiceException
{
    public AdminProtectedException() : base(Resources.AdminProtectedException, StatusCodes.Status400BadRequest)
    {
    }
}
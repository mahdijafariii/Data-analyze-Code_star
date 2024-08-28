namespace AnalysisData.Exception;

public class AdminExistenceException: ServiceException
{
    public AdminExistenceException() : base(Resources.AdminExistenceException,StatusCodes.Status400BadRequest)
    {
    }
}
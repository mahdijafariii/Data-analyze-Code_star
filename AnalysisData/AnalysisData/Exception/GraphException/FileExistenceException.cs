namespace AnalysisData.Exception;

public class FileExistenceException : ServiceException
{
    public FileExistenceException() : base(Resources.FileExistenceException, StatusCodes.Status400BadRequest)
    {
    }
}
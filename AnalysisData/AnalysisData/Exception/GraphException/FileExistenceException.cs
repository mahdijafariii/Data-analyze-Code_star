namespace AnalysisData.Exception.GraphException;

public class FileExistenceException : ServiceException
{
    public FileExistenceException() : base(Resources.FileExistenceException, StatusCodes.Status400BadRequest)
    {
    }
}
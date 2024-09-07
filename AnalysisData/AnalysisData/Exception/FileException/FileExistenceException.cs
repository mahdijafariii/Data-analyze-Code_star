namespace AnalysisData.Exception.FileException;

public class FileExistenceException : ServiceException
{
    public FileExistenceException() : base(Resources.FileExistenceException, StatusCodes.Status400BadRequest)
    {
    }
}
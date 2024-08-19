namespace AnalysisData.Exception;

public class FileExistenceException : System.Exception
{
    public FileExistenceException() : base(Resources.FileExistenceException)
    {
    }
}
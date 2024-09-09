namespace AnalysisData.Exception.FileException;

public class FileProcessingException : ServiceException
{
    public FileProcessingException(string missingHeaders)
        : base(string.Format(Resources.FileProcessingException, string.Join(", ", missingHeaders)), StatusCodes.Status404NotFound)
    {
    }
}
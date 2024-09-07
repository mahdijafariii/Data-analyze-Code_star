namespace AnalysisData.Exception.FileException;

public class NoFileUploadedException : ServiceException
{
    public NoFileUploadedException() : base(Resources.NoFileUploadedException, StatusCodes.Status422UnprocessableEntity)
    {
    }
}
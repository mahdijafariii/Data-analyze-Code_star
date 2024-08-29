namespace AnalysisData.Exception;

public class NoFileUploadedException : ServiceException
{
    public NoFileUploadedException() : base(Resources.NoFileUploadedException, StatusCodes.Status422UnprocessableEntity)
    {
    }
}
namespace AnalysisData.Exception;

public class OperationFailedException : ServiceException
{
    public OperationFailedException() : base(Resources.FailedOpetaionException, StatusCodes.Status400BadRequest)
    {
    }
}
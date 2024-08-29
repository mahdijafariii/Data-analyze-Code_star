namespace AnalysisData.Exception;

public class GuidNotCorrectFormat : ServiceException
{
    public GuidNotCorrectFormat() : base(Resources.GuidNotCorrectFormat,StatusCodes.Status401Unauthorized)
    {
    }
}
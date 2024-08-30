namespace AnalysisData.Exception;

public class GuidNotCorrectFormat : ServiceException
{
    public GuidNotCorrectFormat() : base(Resources.GuidNotCorrectFormatException,StatusCodes.Status400BadRequest)
    {
    }
}
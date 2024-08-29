namespace AnalysisData.Exception;

public class NodeNotFoundException : ServiceException
{
    public NodeNotFoundException() : base(Resources.NodeNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
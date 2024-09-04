namespace AnalysisData.Exception.GraphException;

public class NodeNotFoundException : ServiceException
{
    public NodeNotFoundException() : base(Resources.NodeNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
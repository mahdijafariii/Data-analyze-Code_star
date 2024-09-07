namespace AnalysisData.Exception.GraphException.NodeException;

public class NodeNotFoundException : ServiceException
{
    public NodeNotFoundException() : base(Resources.NodeNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
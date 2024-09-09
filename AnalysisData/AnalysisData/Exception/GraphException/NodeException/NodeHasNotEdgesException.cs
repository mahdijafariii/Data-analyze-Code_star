namespace AnalysisData.Exception.GraphException.NodeException;

public class NodeHasNotEdgesException : ServiceException
{
    public NodeHasNotEdgesException() : base(Resources.NodeHasNotEdgesException,
        StatusCodes.Status404NotFound)
    {
    }
}
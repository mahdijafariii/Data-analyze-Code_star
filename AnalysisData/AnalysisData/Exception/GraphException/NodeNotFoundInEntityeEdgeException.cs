namespace AnalysisData.Exception;


public class NodeNotFoundInEntityEdgeException : ServiceException
{
    public NodeNotFoundInEntityEdgeException(IEnumerable<string> ids) 
        : base(string.Format(Resources.NodeNotFoundInEntityEdgeException, string.Join(", ", ids)), StatusCodes.Status404NotFound)
    {
    }
}

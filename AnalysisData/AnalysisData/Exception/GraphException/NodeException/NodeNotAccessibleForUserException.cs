namespace AnalysisData.Exception.GraphException.NodeException;

public class NodeNotAccessibleForUserException:ServiceException
{
    public NodeNotAccessibleForUserException() : base(Resources.NodeNotAccessibleForUserException,
        StatusCodes.Status404NotFound)
    {
    }
}
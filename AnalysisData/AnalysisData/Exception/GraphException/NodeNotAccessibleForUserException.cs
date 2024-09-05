namespace AnalysisData.Exception.GraphException;

public class NodeNotAccessibleForUserException:ServiceException
{
    public NodeNotAccessibleForUserException() : base(Resources.NodeNotAccessibleForUserException,
        StatusCodes.Status404NotFound)
    {
    }
}
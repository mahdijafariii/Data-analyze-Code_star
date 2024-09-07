namespace AnalysisData.Exception.GraphException.EdgeException;

public class EdgeNotFoundException : ServiceException
{
    public EdgeNotFoundException() : base(Resources.EdgeNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
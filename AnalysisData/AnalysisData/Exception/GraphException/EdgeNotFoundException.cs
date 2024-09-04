namespace AnalysisData.Exception.GraphException;

public class EdgeNotFoundException : ServiceException
{
    public EdgeNotFoundException() : base(Resources.EdgeNotFoundException, StatusCodes.Status404NotFound)
    {
    }
}
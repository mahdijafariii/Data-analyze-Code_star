namespace AnalysisData.Exception.GraphException;

public class CategoryResultNotFoundException : ServiceException
{
    public CategoryResultNotFoundException() : base(Resources.CategoryResultNotFoundException,
        StatusCodes.Status404NotFound)
    {
    }
}
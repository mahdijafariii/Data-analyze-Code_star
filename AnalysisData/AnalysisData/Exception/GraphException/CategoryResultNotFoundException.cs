namespace AnalysisData.Exception;

public class CategoryResultNotFoundException : ServiceException
{
    public CategoryResultNotFoundException() : base(Resources.CategoryResultNotFoundException,
        StatusCodes.Status404NotFound)
    {
    }
}
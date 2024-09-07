namespace AnalysisData.Exception.GraphException.CategoryException;

public class CategoryAlreadyExist : ServiceException
{
    public CategoryAlreadyExist() : base(Resources.CategoryAlreadyExist, StatusCodes.Status400BadRequest)
    {
    }
}
namespace AnalysisData.Exception.GraphException;

public class CategoryAlreadyExist : ServiceException
{
    public CategoryAlreadyExist() : base(Resources.CategoryAlreadyExist, StatusCodes.Status400BadRequest)
    {
    }
}
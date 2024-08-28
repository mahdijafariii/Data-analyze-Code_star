namespace AnalysisData.Exception;

public class CategoryAlreadyExist : ServiceException
{
    public CategoryAlreadyExist() : base(Resources.CategoryAlreadyExist,StatusCodes.Status400BadRequest)
    {
    }

}
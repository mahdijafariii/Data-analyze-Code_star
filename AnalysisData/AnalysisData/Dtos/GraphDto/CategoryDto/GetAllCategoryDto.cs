using AnalysisData.Models.GraphModel.Category;

namespace AnalysisData.Dtos.GraphDto.CategoryDto;

public class GetAllCategoryDto
{
    public IEnumerable<Category> Categories;
}
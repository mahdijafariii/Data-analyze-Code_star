using AnalysisData.Graph.Dto.CategoryDto;
using AnalysisData.Graph.Model.Category;

namespace AnalysisData.Graph.Service.CategoryService.Abstraction;

public interface ICategoryService
{
    Task<PaginationCategoryDto> GetAllCategoriesAsync(int pageNumber, int pageSize);
    Task AddAsync(NewCategoryDto categoryDto);
    Task UpdateAsync(NewCategoryDto newCategoryDto, int preCategoryId);
    Task DeleteAsync(int id);
    Task<Category> GetByIdAsync(int id);
}
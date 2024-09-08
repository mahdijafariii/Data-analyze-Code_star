using AnalysisData.Dtos.GraphDto.CategoryDto;
using AnalysisData.Models.GraphModel.Category;

namespace AnalysisData.Services.GraphService.CategoryService.Abstraction;

public interface ICategoryService
{
    Task<PaginationCategoryDto> GetAllCategoriesAsync(int pageNumber, int pageSize);
    Task AddAsync(NewCategoryDto categoryDto);
    Task UpdateAsync(UpdateCategoryDto newCategoryDto);
    Task DeleteAsync(int id);
    Task<Category> GetByIdAsync(int id);
}
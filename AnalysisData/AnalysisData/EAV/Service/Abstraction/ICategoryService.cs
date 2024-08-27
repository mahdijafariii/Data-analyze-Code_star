using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Service.Abstraction;

public interface ICategoryService
{
    Task<PaginationCategoryDto> GetPaginatedCategoriesAsync(int pageNumber, int pageSize);
    Task AddCategoryAsync(AddCategoryDto categoryDto);
    Task UpdateCategoryAsync(AddCategoryDto newCategoryDto, int preCategoryId);
    Task DeleteCategoryAsync(int id);
    Task<Category> GetCategoryByIdAsync(int id);
}
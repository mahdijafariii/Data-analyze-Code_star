using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Service.Abstraction;

public interface ICategoryService
{
    Task<PaginationCategoryDto> GetAllCategoriesAsync(int pageNumber, int pageSize);
    Task AddAsync(NewCategoryDto categoryDto);
    Task UpdateAsync(NewCategoryDto newCategoryDto, int preCategoryId);
    Task DeleteAsync(int id);
    Task<Category> GetByIdAsync(int id);
}
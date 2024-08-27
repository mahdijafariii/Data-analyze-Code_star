using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.CategoryRepository.asbtraction;
using AnalysisData.EAV.Service.Abstraction;

namespace AnalysisData.EAV.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUploadDataRepository _uploadDataRepository;


    public CategoryService(ICategoryRepository categoryRepository, IUploadDataRepository uploadDataRepository)
    {
        _categoryRepository = categoryRepository;
        _uploadDataRepository = uploadDataRepository;
    }

    public async Task<PaginationCategoryDto> GetPaginatedCategoriesAsync(int pageNumber, int pageSize)
    {
        var allCategories = await _categoryRepository.GetAllAsync();
        await SetCountOfEachCategory(allCategories);
        var totalCount = allCategories.Count;

        var paginatedItems = allCategories
            .Skip((pageNumber) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginationCategoryDto(paginatedItems, pageNumber, totalCount);
    }

    public async Task AddCategoryAsync(AddCategoryDto categoryDto)
    {
        var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
        if (existingCategory != null)
        {
            throw new InvalidOperationException($"A category with the name '{categoryDto.Name}' already exists.");
        }

        var category = new Category
        {
            Name = categoryDto.Name
        };

        await _categoryRepository.AddAsync(category);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    private async Task SetCountOfEachCategory(List<Category> categories)
    {
        foreach (var category in categories)
        { 
            category.TotalNumber = await _uploadDataRepository.GetNumberOfFileWithCategoryIdAsync(category.Id);
        }
    }
}

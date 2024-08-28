using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.CategoryRepository.asbtraction;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.EAV.Service.Abstraction;
using CsvHelper.TypeConversion;

namespace AnalysisData.EAV.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileUploadedRepository _fileUploadedRepository;



    public CategoryService(ICategoryRepository categoryRepository, IFileUploadedRepository fileUploadedRepository)
    {
        _categoryRepository = categoryRepository;
        _fileUploadedRepository = fileUploadedRepository;
    }

    public async Task<PaginationCategoryDto> GetPaginatedCategoriesAsync(int pageNumber, int pageSize)
    {
        var allCategories = await _categoryRepository.GetAllAsync();
        var allCategoriesDto = await MakeCategoryDto(allCategories);
        var totalCount = allCategories.Count();

        var paginatedItems = allCategoriesDto
            .Skip((pageNumber) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginationCategoryDto(paginatedItems, pageNumber, totalCount);
    }

    public async Task AddCategoryAsync(NewCategoryDto categoryDto)
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

    public async Task UpdateCategoryAsync(NewCategoryDto newCategoryDto, int preCategoryId)
    {
        var currentCategory = await _categoryRepository.GetByIdAsync(preCategoryId);
        var existingCategory = await _categoryRepository.GetByNameAsync(newCategoryDto.Name);
        if (existingCategory != null && newCategoryDto.Name != currentCategory.Name)
        {
            throw new InvalidOperationException($"A category with the name '{newCategoryDto.Name}' already exists.");
        }

        currentCategory.Name = newCategoryDto.Name;
        await _categoryRepository.UpdateAsync(currentCategory);
    }


    public async Task DeleteCategoryAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    private async Task<IEnumerable<CategoryDto>> MakeCategoryDto(IEnumerable<Category> categories)
    {
        var categoryDtoTasks = categories.Select(async category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            TotalNumber = await _fileUploadedRepository.GetNumberOfFileWithCategoryIdAsync(category.Id)
        });

        return await Task.WhenAll(categoryDtoTasks);
    }
}
using AnalysisData.Dtos.GraphDto.CategoryDto;
using AnalysisData.Exception.GraphException.CategoryException;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Repositories.GraphRepositories.CategoryRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository.Abstraction;
using AnalysisData.Services.GraphService.CategoryService.Abstraction;

namespace AnalysisData.Services.GraphService.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileUploadedRepository _fileUploadedRepository;


    public CategoryService(ICategoryRepository categoryRepository, IFileUploadedRepository fileUploadedRepository)
    {
        _categoryRepository = categoryRepository;
        _fileUploadedRepository = fileUploadedRepository;
    }

    public async Task<PaginationCategoryDto> GetAllCategoriesAsync(int pageNumber, int pageSize)
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
    
    public async Task<GetAllCategoryDto> GetAllCategoriesWithoutPaginationAsync()
    {
        var allCategoryDto = await _categoryRepository.GetAllAsync();
        return new GetAllCategoryDto(allCategoryDto);
    }

    public async Task AddAsync(NewCategoryDto categoryDto)
    {
        var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
        if (existingCategory != null)
        {
            throw new CategoryAlreadyExist();
        }

        var category = new Category
        {
            Name = categoryDto.Name
        };

        await _categoryRepository.AddAsync(category);
    }

    public async Task UpdateAsync(UpdateCategoryDto updateCategoryDto)
    {
        var currentCategory = await _categoryRepository.GetByIdAsync(updateCategoryDto.Id);
        var existingCategory = await _categoryRepository.GetByNameAsync(updateCategoryDto.Name);
        if (existingCategory != null && updateCategoryDto.Name != currentCategory.Name)
        {
            throw new CategoryAlreadyExist();
        }

        currentCategory.Name = updateCategoryDto.Name;
        await _categoryRepository.UpdateAsync(currentCategory);
    }


    public async Task DeleteAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    private async Task<IEnumerable<CategoryDto>> MakeCategoryDto(IEnumerable<Category> categories)
    {
        var categoryDtoList = new List<CategoryDto>();

        foreach (var category in categories)
        {
            var totalNumber = await _fileUploadedRepository.GetNumberOfFileWithCategoryIdAsync(category.Id);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                TotalNumber = totalNumber
            };

            categoryDtoList.Add(categoryDto);
        }

        return categoryDtoList;
    }
}
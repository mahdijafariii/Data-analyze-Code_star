using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Dto.CategoryDto;
using AnalysisData.Graph.Model.Category;
using AnalysisData.Graph.Repository.CategoryRepository.Abstraction;
using AnalysisData.Graph.Repository.FileUploadedRepository.Abstraction;
using AnalysisData.Graph.Service.CategoryService.Abstraction;

namespace AnalysisData.Graph.Service.CategoryService;

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

    public async Task UpdateAsync(NewCategoryDto newCategoryDto, int preCategoryId)
    {
        var currentCategory = await _categoryRepository.GetByIdAsync(preCategoryId);
        var existingCategory = await _categoryRepository.GetByNameAsync(newCategoryDto.Name);
        if (existingCategory != null && newCategoryDto.Name != currentCategory.Name)
        {
            throw new CategoryAlreadyExist();
        }

        currentCategory.Name = newCategoryDto.Name;
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
using AnalysisData.Dtos.GraphDto.CategoryDto;
using AnalysisData.Services.GraphService.CategoryService.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers.GraphController;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [Authorize(Policy = "silver")]
    [HttpGet]
    public async Task<IActionResult> GetCategories(int pageNumber = 0, int pageSize = 10)
    {
        var paginatedCategories = await _categoryService.GetAllCategoriesAsync(pageNumber, pageSize);
        return Ok(paginatedCategories);
    }
    
    [Authorize(Policy = "silver")]
    [HttpGet("all-category-without-pagination")]
    public async Task<IActionResult> GetAllCategoriesWithOutPagination()
    {
        var categories = await _categoryService.GetAllCategoriesWithoutPaginationAsync();
        return Ok(categories);
    }

    [Authorize(Policy = "silver")]
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] NewCategoryDto categoryDto)
    {
        await _categoryService.AddAsync(categoryDto);
        return Ok(new { message = "Category added!" });
    }
    
    [Authorize(Policy = "silver")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found." });
        }

        await _categoryService.DeleteAsync(id);
        return NoContent();
    }

    [Authorize(Policy = "silver")]
    [HttpPut]
    public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDto newCategory)
    {
        await _categoryService.UpdateAsync(newCategory);
        return Ok(new { massage = "updated successfully" });
    }
}
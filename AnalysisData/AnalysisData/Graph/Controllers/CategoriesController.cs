using AnalysisData.Graph.Dto.CategoryDto;
using AnalysisData.Graph.Service.CategoryService.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Graph.Controllers;

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
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] NewCategoryDto newCategory)
    {
        await _categoryService.UpdateAsync(newCategory, id);
        return Ok(new { massage = "updated successfully" });
    }
}
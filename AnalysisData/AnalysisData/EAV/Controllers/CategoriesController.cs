using AnalysisData.EAV.Model;
using AnalysisData.EAV.Service;
using AnalysisData.EAV.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(int pageNumber = 0, int pageSize = 10, string typeCategory = "Default")
    {
        var paginatedCategories = await _categoryService.GetPaginatedCategoriesAsync(pageNumber, pageSize);
        return Ok(paginatedCategories);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] Category category)
    {
        if (category == null || string.IsNullOrEmpty(category.Name))
        {
            return BadRequest("Category name is required.");
        }

        await _categoryService.AddCategoryAsync(category);
        return CreatedAtAction(nameof(GetCategories), new { pageNumber = 1, pageSize = 10 }, category);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound($"Category with ID {id} not found.");
        }

        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
}

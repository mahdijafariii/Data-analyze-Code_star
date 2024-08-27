using AnalysisData.EAV.Dto;
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
    public async Task<IActionResult> GetCategories(int pageNumber = 0, int pageSize = 10)
    {
        var paginatedCategories = await _categoryService.GetPaginatedCategoriesAsync(pageNumber, pageSize);
        return Ok(paginatedCategories);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryDto categoryDto)
    {
        if (categoryDto == null || string.IsNullOrEmpty(categoryDto.Name))
        {
            return BadRequest("Category name is required.");
        }

        try
        {
            await _categoryService.AddCategoryAsync(categoryDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message); 
        }

        return Ok("Category added!");
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

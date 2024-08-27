using AnalysisData.EAV.Dto;
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
            return BadRequest(new{message="Category name is required."});
        }

        try
        {
            await _categoryService.AddCategoryAsync(categoryDto);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new{message=ex.Message}); 
        }

        return Ok(new{message="Category added!"});
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound(new{message=$"Category with ID {id} not found."});
        }

        await _categoryService.DeleteCategoryAsync(id);
        return NoContent();
    }
    

    [HttpPut]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] AddCategoryDto newCategory)
    {
        await _categoryService.UpdateCategoryAsync(newCategory, id);
        return Ok(new {massage = "updated successfully"});
    }
}

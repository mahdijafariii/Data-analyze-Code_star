using AnalysisData.Controllers.GraphController;
using AnalysisData.Dtos.GraphDto.CategoryDto;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Services.GraphService.CategoryService.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace TestProject.Graph.Controllers;

public class CategoryControllersTest
{
    private readonly Mock<ICategoryService> _categoryServiceMock;
    private readonly CategoriesController _controller;

    public CategoryControllersTest()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _controller = new CategoriesController(_categoryServiceMock.Object);
    }

    [Fact]
    public async Task GetCategories_ShouldReturnOkResult_WhenCategoriesAreFound()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 10;
        var categories = new List<CategoryDto>
        {
            new CategoryDto { Id = 1, Name = "Category1" },
            new CategoryDto { Id = 2, Name = "Category2" }
        };
        PaginationCategoryDto paginationCategoryDto = new PaginationCategoryDto(categories,pageNumber,2);
        _categoryServiceMock.Setup(service => service.GetAllCategoriesAsync(pageNumber, pageSize))
            .ReturnsAsync(paginationCategoryDto);

        // Act
        var result = await _controller.GetCategories(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedCategories = Assert.IsType<PaginationCategoryDto>(okResult.Value);
        Assert.Equal(paginationCategoryDto.PaginateList.Count, returnedCategories.PaginateList.Count());
        Assert.Equivalent(paginationCategoryDto.PaginateList, returnedCategories.PaginateList);
    }
    
    
    [Fact]
    public async Task AddCategory_ShouldReturnOk_WhenCategoryIsAddedSuccessfully()
    {
        // Arrange
        var categoryDto = new NewCategoryDto { Name = "NewCategory" };

        // Act
        var result = await _controller.AddCategory(categoryDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = "Category added!" });
        Assert.Equal(expectedResponseContent, responseContent);

        _categoryServiceMock.Verify(service => service.AddAsync(categoryDto), Times.Once);
    }
    
    [Fact]
    public async Task DeleteCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        int categoryId = 1;
        _categoryServiceMock.Setup(service => service.GetByIdAsync(categoryId))
            .ReturnsAsync((Category)null); 
    
        // Act
        var result = await _controller.DeleteCategory(categoryId);
    
        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(notFoundResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = $"Category with ID {categoryId} not found." });
        Assert.Equal(expectedResponseContent, responseContent);
    
        _categoryServiceMock.Verify(service => service.DeleteAsync(categoryId), Times.Never);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnNoContent_WhenCategoryExists()
    {
        // Arrange
        int categoryId = 1;
        var categoryDto = new Category { Id = categoryId, Name = "ExistingCategory" };
        _categoryServiceMock.Setup(service => service.GetByIdAsync(categoryId))
            .ReturnsAsync(categoryDto); 

        // Act
        var result = await _controller.DeleteCategory(categoryId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _categoryServiceMock.Verify(service => service.DeleteAsync(categoryId), Times.Once);
    }
    
    [Fact]
    public async Task UpdateCategory_ShouldReturnOk_WhenCategoryIsUpdatedSuccessfully()
    {
        // Arrange
        int categoryId = 1;
        var newCategoryDto = new NewCategoryDto { Name = "UpdatedCategory" };

        // Act
        var result = await _controller.UpdateCategory(categoryId, newCategoryDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { massage = "updated successfully" });
        Assert.Equal(expectedResponseContent, responseContent);

        _categoryServiceMock.Verify(service => service.UpdateAsync(newCategoryDto, categoryId), Times.Once);
    }

    
}
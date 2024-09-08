using System.Security.Claims;
using AnalysisData.Exception.GraphException.CategoryException;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using AnalysisData.Services.GraphService.CategoryService.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.AllNodesData;
using NSubstitute;

namespace TestProject.Services.GraphService.GraphServices.AllNodesData;

public class NodePaginationServiceTests
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly ICategoryService _categoryService;
    private readonly NodePaginationService _sut;

    public NodePaginationServiceTests()
    {
        _graphNodeRepository = Substitute.For<IGraphNodeRepository>();
        _categoryService = Substitute.For<ICategoryService>();
        _sut = new NodePaginationService(_graphNodeRepository, _categoryService);
    }

    [Fact]
    public async Task GetAllNodesAsync_ShouldReturnPaginatedNodes_WhenCalled()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var categoryId = 1;
        var pageIndex = 0;
        var pageSize = 10;
        var category = new Category { Name = "Category1" };
        var nodes = new List<EntityNode>
        {
            new EntityNode { Id = Guid.NewGuid(), Name = "Node1" },
            new EntityNode { Id = Guid.NewGuid(), Name = "Node2" }
        };

        _categoryService.GetByIdAsync(categoryId).Returns(Task.FromResult(category));
        _graphNodeRepository.GetEntityNodesForAdminWithCategoryIdAsync(categoryId).Returns(Task.FromResult(nodes.AsEnumerable()));

        // Act
        var result = await _sut.GetAllNodesAsync(claimsPrincipal, pageIndex, pageSize, categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count); 
        Assert.Equal("Category1", result.CategoryName);
    }

    [Fact]
    public async Task GetAllNodesAsync_ShouldThrowCategoryResultNotFoundException_WhenCategoryNotFound()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var categoryId = 1;
        _categoryService.GetByIdAsync(categoryId).Returns(Task.FromResult<Category>(null));

        // Act
        Exception exception = await Record.ExceptionAsync(() => _sut.GetAllNodesAsync(claimsPrincipal, 0, 10, categoryId));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<CategoryResultNotFoundException>(exception);
    }
    
    [Fact]
    public async Task GetAllNodesAsync_ShouldReturnNodesForDataAnalystRole_WhenCalled()
    {
        // Arrange
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "dataanalyst"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var categoryId = 1;
        var pageIndex = 0;
        var pageSize = 10;
        var nodes = new List<EntityNode>
        {
            new EntityNode { Id = Guid.NewGuid(), Name = "Node1" },
            new EntityNode { Id = Guid.NewGuid(), Name = "Node2" }
        };

        var usernameGuid = Guid.Parse(claimsPrincipal.FindFirstValue("id"));
        _graphNodeRepository.GetEntityNodeForUserWithCategoryIdAsync(usernameGuid, categoryId).Returns(Task.FromResult(nodes.AsEnumerable()));

        // Act
        var result = await _sut.GetAllNodesAsync(claimsPrincipal, pageIndex, pageSize, categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count); 
    }
}

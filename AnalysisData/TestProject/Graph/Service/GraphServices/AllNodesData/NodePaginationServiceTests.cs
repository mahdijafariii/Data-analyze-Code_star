using System.Security.Claims;
using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Category;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.GraphNodeRepository;
using AnalysisData.Graph.Service.CategoryService.Abstraction;
using AnalysisData.Graph.Service.GraphServices.AllNodesData;
using NSubstitute;

namespace TestProject.Graph.Service.GraphServices.AllNodesData;

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

    private ClaimsPrincipal CreateClaimsPrincipal(string role, string userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, role),
            new("id", userId)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        return new ClaimsPrincipal(identity);
    }
    
    [Fact]
    public async Task GetAllNodesAsync_ShouldReturnsPaginatedNodesForUser_WhenNodesExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var categoryId = 1;

        var entityNodes = new List<EntityNode>
        {
            new() { Id = 1, Name = "Node1" },
            new() { Id = 2, Name = "Node2" }
        };

        _graphNodeRepository.GetEntityNodesForUserAsync(Arg.Any<Guid>())
            .Returns(entityNodes);

        _categoryService.GetByIdAsync(categoryId)
            .Returns(Task.FromResult(new Category { Id = categoryId, Name = "Category1" }));

        // Act
        var result = await _sut.GetAllNodesAsync(claimsPrincipal, 0, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
    }
    
    [Fact]
    public async Task GetAllNodesAsync_ShouldReturnsPaginatedNodesForUserWithCategoryId_WhenNodesExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var categoryId = 1;

        var entityNodes = new List<EntityNode>
        {
            new() { Id = 1, Name = "Node1" },
            new() { Id = 2, Name = "Node2" }
        };

        _graphNodeRepository.GetEntityNodeForUserWithCategoryIdAsync(Arg.Any<Guid>(), categoryId)
            .Returns(entityNodes);

        _categoryService.GetByIdAsync(categoryId)
            .Returns(Task.FromResult(new Category { Id = categoryId, Name = "Category1" }));

        // Act
        var result = await _sut.GetAllNodesAsync(claimsPrincipal, 0, 10, categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Category1", result.CategoryName);
        Assert.Equal(2, result.Items.Count);
    }


    [Fact]
    public async Task GetAllNodesAsync_ShouldReturnsPaginatedNodesForAdminWithCategoryId_WhenNodesExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var categoryId = 1;

        var entityNodes = new List<EntityNode>
        {
            new() { Id = 1, Name = "Node1" },
            new() { Id = 2, Name = "Node2" }
        };

        _graphNodeRepository.GetEntityNodesForAdminWithCategoryIdAsync(categoryId)
            .Returns(entityNodes);

        _categoryService.GetByIdAsync(categoryId)
            .Returns(Task.FromResult(new Category { Id = categoryId, Name = "Category1" }));

        // Act
        var result = await _sut.GetAllNodesAsync(claimsPrincipal, 0, 10, categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Category1", result.CategoryName);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetAllNodesAsync_ShouldReturnsPaginatedNodesForAdmin_WhenNodesExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var categoryId = 1;

        var entityNodes = new List<EntityNode>
        {
            new() { Id = 1, Name = "Node1" },
            new() { Id = 2, Name = "Node2" }
        };

        _graphNodeRepository.GetEntityNodesForAdminAsync()
            .Returns(entityNodes);

        _categoryService.GetByIdAsync(categoryId)
            .Returns(Task.FromResult(new Category { Id = categoryId, Name = "Category1" }));

        // Act
        var result = await _sut.GetAllNodesAsync(claimsPrincipal, 0, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
    }


    [Fact]
    public async Task GetAllNodesAsync_ShouldThrowsNodeNotFoundException_WhenNoNodesExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        _graphNodeRepository.GetEntityNodesForUserAsync(Arg.Any<Guid>())
            .Returns(Enumerable.Empty<EntityNode>());

        // Act
        var action = () => _sut.GetAllNodesAsync(claimsPrincipal, 0, 10);

        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
    }

    [Fact]
    public async Task GetAllNodesAsync_shouldThrowsCategoryResultNotFoundException_WhenNoNodesExistForCategory()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var categoryId = 1;


        _categoryService.GetByIdAsync(categoryId)
            .Returns(Task.FromResult(new Category { Id = categoryId, Name = "Category1" }));


        // Act
        var action = () => _sut.GetAllNodesAsync(claimsPrincipal, 0, 10, categoryId);

        // Assert
        await Assert.ThrowsAsync<CategoryResultNotFoundException>(action);
    }
}
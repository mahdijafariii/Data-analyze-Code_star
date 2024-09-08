using System.Security.Claims;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Search;
using NSubstitute;

namespace TestProject.Services.GraphService.GraphServices.Search;

public class GraphSearchServiceTests
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly GraphSearchService _sut;

    public GraphSearchServiceTests()
    {
        _graphNodeRepository = Substitute.For<IGraphNodeRepository>();
        _sut = new GraphSearchService(_graphNodeRepository);
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
    public async Task SearchInEntityNodeNameAsync_ShouldReturnResults_WhenRoleIsAdminAndSearchTypeStartsWith()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var searchInput = "Node";
        var searchType = "startswith";

        var entityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "Node1" },
            new() { Id = Guid.NewGuid(), Name = "Node2" }
        };
        _graphNodeRepository.GetNodeStartsWithSearchInputForAdminAsync(searchInput).Returns(entityNodes);

        // Act
        var result = await _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);

        // Assert
        Assert.Equal(entityNodes.Count, result.Items.Count);
        await _graphNodeRepository.Received(1).GetNodeStartsWithSearchInputForAdminAsync(searchInput);
    }
    
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldReturnResults_WhenRoleIsAdminAndSearchTypeContains()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var searchInput = "Node";
        var searchType = "contains";

        var entityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "TheNode1" },
            new() { Id = Guid.NewGuid(), Name = "NodeFound" }
        };
        _graphNodeRepository.GetNodeContainSearchInputForAdminAsync(searchInput).Returns(entityNodes);

        // Act
        var result = await _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);

        // Assert
        Assert.Equal(entityNodes.Count, result.Items.Count);
        await _graphNodeRepository.Received(1).GetNodeContainSearchInputForAdminAsync(searchInput);
    }
    
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldReturnResults_WhenRoleIsAdminAndSearchTypeEndsWith()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var searchInput = "Node";
        var searchType = "endswith";

        var entityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "MyNode" },
            new() { Id = Guid.NewGuid(), Name = "YourNode" }
        };
        _graphNodeRepository.GetNodeEndsWithSearchInputForAdminAsync(searchInput).Returns(entityNodes);

        // Act
        var result = await _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);

        // Assert
        Assert.Equal(entityNodes.Count, result.Items.Count);
        await _graphNodeRepository.Received(1).GetNodeEndsWithSearchInputForAdminAsync(searchInput);
    }
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldReturnResults_WhenRoleIsUserAndSearchTypeStartsWith()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var searchInput = "Node";
        var searchType = "startswith";

        var entityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "Node1" },
            new() { Id = Guid.NewGuid(), Name = "Node2" }
        };
        _graphNodeRepository.GetNodeStartsWithSearchInputForUserAsync(Arg.Any<Guid>(),searchInput).Returns(entityNodes);

        // Act
        var result = await _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);

        // Assert
        Assert.Equal(entityNodes.Count, result.Items.Count);
        await _graphNodeRepository.Received(1).GetNodeStartsWithSearchInputForUserAsync(Arg.Any<Guid>(),searchInput);
    }
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldReturnResults_WhenRoleIsUserAndSearchTypeContains()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var searchInput = "Node";
        var searchType = "contains";

        var entityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "NodeA" },
            new() { Id = Guid.NewGuid(), Name = "BNodeB" }
        };
        _graphNodeRepository.GetNodeContainSearchInputForUserAsync(Arg.Any<Guid>(), searchInput).Returns(entityNodes);

        // Act
        var result = await _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);

        // Assert
        Assert.Equal(entityNodes.Count, result.Items.Count);
        await _graphNodeRepository.Received(1).GetNodeContainSearchInputForUserAsync(Arg.Any<Guid>(), searchInput);
    }

    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldReturnResults_WhenRoleIsUserAndSearchTypeEndsWith()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var searchInput = "Node";
        var searchType = "endswith";

        var entityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "MyNode" },
            new() { Id = Guid.NewGuid(), Name = "YourNode" }
        };
        _graphNodeRepository.GetNodeEndsWithSearchInputForUserAsync(Arg.Any<Guid>(), searchInput).Returns(entityNodes);

        // Act
        var result = await _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);

        // Assert
        Assert.Equal(entityNodes.Count, result.Items.Count);
        await _graphNodeRepository.Received(1).GetNodeEndsWithSearchInputForUserAsync(Arg.Any<Guid>(), searchInput);
    }
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldThrowNodeNotFoundException_WhenNoNodesExistForAdmin()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var searchInput = "Node";
        var searchType = "";

        _graphNodeRepository.GetNodeContainSearchInputForAdminAsync(searchInput)
            .Returns(Enumerable.Empty<EntityNode>());

        // Act
        var action = () => _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,0,10);
        
        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
        await _graphNodeRepository.Received(1).GetNodeContainSearchInputForAdminAsync(searchInput);
    }
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ShouldThrowNodeNotFoundException_WhenNoNodesExistForUser()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var searchInput = "Node";
        var searchType = "";

        _graphNodeRepository.GetNodeStartsWithSearchInputForUserAsync(Arg.Any<Guid>(), searchInput)
            .Returns(Enumerable.Empty<EntityNode>());

        // Act
        var action =()=> _sut.SearchInEntityNodeNameAsync(claimsPrincipal, searchInput, searchType,1,10);
        
        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
        await _graphNodeRepository.Received(1).GetNodeContainSearchInputForUserAsync(Arg.Any<Guid>(), searchInput);
    }
}
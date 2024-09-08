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
    private readonly GraphSearchService _graphSearchService;

    public GraphSearchServiceTests()
    {
        _graphNodeRepository = Substitute.For<IGraphNodeRepository>();
        _graphSearchService = new GraphSearchService(_graphNodeRepository);
    }

    [Fact]
    public async Task SearchInEntityNodeNameAsync_AdminRole_ReturnsNodes_WhenNodesExist()
    {
        // Arrange
        var claimsPrincipal = CreateClaimsPrincipal("admin", "1234");
        var inputSearch = "searchInput";
        var type = "startswith";
        var expectedNodes = new List<EntityNode> { new EntityNode() };

        _graphNodeRepository.GetNodeStartsWithSearchInputForAdminAsync(inputSearch)
            .Returns(Task.FromResult(expectedNodes.AsEnumerable()));

        // Act
        var result = await _graphSearchService.SearchInEntityNodeNameAsync(claimsPrincipal, inputSearch, type);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(expectedNodes, result);
    }
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_UserRole_ReturnsNodes_WhenNodesExist()
    {
        // Arrange
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", "023642e0-7f63-41c7-a447-c47bcf608384");
        var inputSearch = "searchInput";
        var type = "endswith";
        var expectedNodes = new List<EntityNode> { new EntityNode() };
        var usernameGuid = Guid.Parse("023642e0-7f63-41c7-a447-c47bcf608384");

        _graphNodeRepository.GetNodeEndsWithSearchInputForUserAsync(usernameGuid, inputSearch)
            .Returns(Task.FromResult(expectedNodes.AsEnumerable()));

        // Act
        var result = await _graphSearchService.SearchInEntityNodeNameAsync(claimsPrincipal, inputSearch, type);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(expectedNodes, result);
    }
    
    [Fact]
    public async Task SearchInEntityNodeNameAsync_ThrowsNodeNotFoundException_WhenNoNodesFound()
    {
        // Arrange
        var claimsPrincipal = CreateClaimsPrincipal("admin", "023642e0-7f63-41c7-a447-c47bcf608384");
        var inputSearch = "searchInput";
        var type = "startswith";

        _graphNodeRepository.GetNodeStartsWithSearchInputForAdminAsync(inputSearch)
            .Returns(Task.FromResult(Enumerable.Empty<EntityNode>()));

        // Act
        Task Act() => _graphSearchService.SearchInEntityNodeNameAsync(claimsPrincipal, inputSearch, type);

        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(Act);
    }
    private ClaimsPrincipal CreateClaimsPrincipal(string role, string username)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, role),
            new Claim("id", username)
        };
        var identity = new ClaimsIdentity(claims);
        return new ClaimsPrincipal(identity);
    }
}
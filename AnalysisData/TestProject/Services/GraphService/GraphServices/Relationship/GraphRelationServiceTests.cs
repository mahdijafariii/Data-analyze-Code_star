using System.Security.Claims;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Relationship;
using NSubstitute;

namespace TestProject.Services.GraphService.GraphServices.Relationship;

public class GraphRelationServiceTests
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly GraphRelationService _sut;

    public GraphRelationServiceTests()
    {
        _graphNodeRepository = Substitute.For<IGraphNodeRepository>();
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _entityEdgeRepository = Substitute.For<IEntityEdgeRepository>();
        _sut = new GraphRelationService(_entityNodeRepository, _entityEdgeRepository, _graphNodeRepository);
    }

    private ClaimsPrincipal CreateClaimsPrincipal(string role, string id)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Role, role),
            new("id", id)
        };
        return new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));
    }
    
    
    [Fact]
    public async Task GetRelationalEdgeBaseNodeAsync_ShouldThrowNodeNotAccessibleForUserException_WhenNodeIsNotAccessible()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claimsPrincipal = CreateClaimsPrincipal("data-analyst", userId.ToString());
        var nodeId = Guid.NewGuid();

        var node = new EntityNode { Id = nodeId, Name = "Node1" };
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(node);
        _graphNodeRepository.IsNodeAccessibleByUser(userId, nodeId).Returns(false);

        // Act
        var action = async () => await _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);

        // Assert
        await Assert.ThrowsAsync<NodeNotAccessibleForUserException>(action);
    }

    [Fact]
    public async Task GetRelationalEdgeBaseNodeAsync_ShouldThrowNodeNotFoundException_WhenNoNodesAndEdgesReturned()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var nodeId = Guid.NewGuid();

        var node = new EntityNode { Id = nodeId, Name = "Node1" };
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(node);

        var edges = new List<EntityEdge>();
        _entityEdgeRepository.FindNodeLoopsAsync(nodeId).Returns(edges);

        // Act
        var action = async () => await _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);

        // Assert
        await Assert.ThrowsAsync<NodeHasNotEdgesException>(action);
    } 
    
    [Fact]
    public async Task GetNodeRelationsAsync_ShouldThrowNodeNotFoundException_WhenNodeDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var nodeId = Guid.NewGuid();
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(Task.FromResult<EntityNode>(null));

        // Act
        var action = async () => await _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);

        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
    }
}

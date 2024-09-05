using System.Security.Claims;
using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.GraphNodeRepository;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.GraphServices.Relationship;
using NSubstitute;

namespace TestProject.Graph.Service.GraphServices.Relationship;

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
    public async Task GetRelationalEdgeBaseNodeAsync_ShouldReturnNodesAndEdges_WhenUserIsAdmin()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        
        var nodeId = 1;
        var node = new EntityNode { Id = nodeId, Name = "Node1" };
        
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(node);

        var edges = new List<EntityEdge>
        {
            new() { Id = 1, EntityIDSource = 1, EntityIDTarget = 2 },
            new() { Id = 2, EntityIDSource = 1, EntityIDTarget = 3 }
        };
        _entityEdgeRepository.FindNodeLoopsAsync(nodeId).Returns(edges);

        var nodes = new List<EntityNode>
        {
            new() { Id = 1, Name = "Node1" },
            new() { Id = 2, Name = "Node2" },
            new() { Id = 3, Name = "Node3" }
        };
        _entityNodeRepository.GetByIdAsync(Arg.Any<int>()).Returns(args => Task.FromResult(nodes.First(n => n.Id == args.Arg<int>())));

        // Act
        var result = await _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);

        // Assert
        Assert.Equal(3, result.Item1.Count());
        Assert.Equal(2, result.Item2.Count());
    }

    [Fact]
    public async Task GetRelationalEdgeBaseNodeAsync_ShouldReturnNodesAndEdges_WhenUserIsDataAnalystAndNodeIsAccessible()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId.ToString());
        
        var nodeId = 1;
        var node = new EntityNode { Id = nodeId, Name = "Node1" };
        
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(node);
        _graphNodeRepository.IsNodeAccessibleByUser(userId, nodeId).Returns(true);

        var edges = new List<EntityEdge>
        {
            new() { Id = 1, EntityIDSource = 1, EntityIDTarget = 2 },
            new() { Id = 2, EntityIDSource = 1, EntityIDTarget = 3 }
        };
        _entityEdgeRepository.FindNodeLoopsAsync(nodeId).Returns(edges);

        var nodes = new List<EntityNode>
        {
            new() { Id = 1, Name = "Node1" },
            new() { Id = 2, Name = "Node2" },
            new() { Id = 3, Name = "Node3" }
        };
        _entityNodeRepository.GetByIdAsync(Arg.Any<int>()).Returns(args => Task.FromResult(nodes.First(n => n.Id == args.Arg<int>())));

        // Act
        var result = await _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Item1.Count());
        Assert.Equal(2, result.Item2.Count());
    }

    [Fact]
    public async Task GetRelationalEdgeBaseNodeAsync_ShouldThrowNodeNotAccessibleForUserException_WhenNodeIsNotAccessible()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId.ToString());
        var nodeId = 1;

        var node = new EntityNode { Id = nodeId, Name = "Node1" };
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(Task.FromResult(node));
        _graphNodeRepository.IsNodeAccessibleByUser(userId, nodeId).Returns(false);

        // Act
        var action = () => _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);
        // Assert
        await Assert.ThrowsAsync<NodeNotAccessibleForUserException>(action);
    }

    [Fact]
    public async Task GetRelationalEdgeBaseNodeAsync_ShouldThrowNodeNotFoundException_WhenNoNodesAndEdgesReturned()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var nodeId = 1;

        var node = new EntityNode { Id = nodeId, Name = "Node1" };
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(Task.FromResult(node));

        var edges = new List<EntityEdge>();
        _entityEdgeRepository.FindNodeLoopsAsync(nodeId).Returns(edges);

        // Act
        var action = () => _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);
        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
    } 
    
    [Fact]
    public async Task GetNodeRelationsAsync_ShouldThrowNodeNotFoundException_WhenNodeDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var nodeId = 1;
        _entityNodeRepository.GetByIdAsync(nodeId).Returns(Task.FromResult<EntityNode>(null!));

        // Act
        var action = () => _sut.GetRelationalEdgeBaseNodeAsync(claimsPrincipal, nodeId);
        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
    }
}
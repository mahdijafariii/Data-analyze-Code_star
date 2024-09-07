using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Exception.GraphException.EdgeException;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using NSubstitute;

namespace TestProject.Graph.Service.GraphServices.NodeAndEdgeInfo;

public class NodeAndEdgeInfoTests
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IGraphEdgeRepository _graphEdgeRepository;
    private readonly AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.NodeAndEdgeInfo _sut;

    public NodeAndEdgeInfoTests()
    {
        _graphNodeRepository = Substitute.For<IGraphNodeRepository>();
        _graphEdgeRepository = Substitute.For<IGraphEdgeRepository>();
        _sut = new AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.NodeAndEdgeInfo(_graphNodeRepository,
            _graphEdgeRepository);
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
    public async Task GetNodeInformationAsync_ShouldReturnsNodeInformation_WhenNodeExistAndUserIsDataAnalyst()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var nodeId = 1;

        var nodeAttributeValues = new List<NodeInformationDto>
        {
            new() { Attribute = "Name1", Value = "Node1" },
            new() { Attribute = "Name2", Value = "Node2" }
        };

        _graphNodeRepository.IsNodeAccessibleByUser(Arg.Any<Guid>(), nodeId)
            .Returns(true);

        _graphNodeRepository.GetNodeAttributeValueAsync(nodeId)
            .Returns(nodeAttributeValues);

        // Act
        var result = await _sut.GetNodeInformationAsync(claimsPrincipal, nodeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Node1", result["Name1"]);
        Assert.Equal("Node2", result["Name2"]);
    }
    
    [Fact]
    public async Task GetNodeInformationAsync_ShouldReturnsNodeInformation_WhenNodeExistAndUserIsNotDataAnalyst()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var nodeId = 1;

        var nodeAttributeValues = new List<NodeInformationDto>
        {
            new() { Attribute = "Name1", Value = "Node1" },
            new() { Attribute = "Name2", Value = "Node2" }
        };

        _graphNodeRepository.IsNodeAccessibleByUser(Arg.Any<Guid>(), nodeId)
            .Returns(true);

        _graphNodeRepository.GetNodeAttributeValueAsync(nodeId)
            .Returns(nodeAttributeValues);

        // Act
        var result = await _sut.GetNodeInformationAsync(claimsPrincipal, nodeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Node1", result["Name1"]);
        Assert.Equal("Node2", result["Name2"]);
    }

    [Fact]
    public async Task GetNodeInformationAsync_ThrowsNodeNotFoundException_WhenNodeDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var nodeId = 1;

        _graphNodeRepository.IsNodeAccessibleByUser(Arg.Any<Guid>(), nodeId)
            .Returns(true);

        _graphNodeRepository.GetNodeAttributeValueAsync(nodeId)
            .Returns(Enumerable.Empty<NodeInformationDto>());

        // Act
        var action = () => _sut.GetNodeInformationAsync(claimsPrincipal, nodeId);
        // Assert
        await Assert.ThrowsAsync<NodeNotFoundException>(action);
    }

    [Fact]
    public async Task GetEdgeInformationAsync_ShouldReturnsEdgeInformation_WhenEdgeExistAndUserIsDataAnalyst()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var edgeId = 1;

        var edgeAttributes = new List<EdgeInformationDto>
        {
            new (){ Attribute = "Name1", Value = "Edge1" },
            new (){ Attribute = "Name2", Value = "Edge2" }
        };

        _graphEdgeRepository.IsEdgeAccessibleByUser(Arg.Any<string>(), edgeId)
            .Returns(true);

        _graphEdgeRepository.GetEdgeAttributeValues(edgeId)
            .Returns(edgeAttributes);

        // Act
        var result = await _sut.GetEdgeInformationAsync(claimsPrincipal, edgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Edge1", result["Name1"]);
        Assert.Equal("Edge2", result["Name2"]);
    }
    
    [Fact]
    public async Task GetEdgeInformationAsync_ShouldReturnsEdgeInformation_WhenEdgeExistAndUserIsNotDataAnalyst()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("admin", userId);
        var edgeId = 1;

        var edgeAttributes = new List<EdgeInformationDto>
        {
            new (){ Attribute = "Name1", Value = "Edge1" },
            new (){ Attribute = "Name2", Value = "Edge2" }
        };

        _graphEdgeRepository.IsEdgeAccessibleByUser(Arg.Any<string>(), edgeId)
            .Returns(true);

        _graphEdgeRepository.GetEdgeAttributeValues(edgeId)
            .Returns(edgeAttributes);

        // Act
        var result = await _sut.GetEdgeInformationAsync(claimsPrincipal, edgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Edge1", result["Name1"]);
        Assert.Equal("Edge2", result["Name2"]);
    }

    [Fact]
    public async Task GetEdgeInformationAsync_ShouldThrowsEdgeNotFoundException_WhenEdgeDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var claimsPrincipal = CreateClaimsPrincipal("dataanalyst", userId);
        var edgeId = 1;

        _graphEdgeRepository.IsEdgeAccessibleByUser(Arg.Any<string>(), edgeId)
            .Returns(true);

        _graphEdgeRepository.GetEdgeAttributeValues(edgeId)
            .Returns(Enumerable.Empty<EdgeInformationDto>());

        // Act
        var action = () => _sut.GetEdgeInformationAsync(claimsPrincipal, edgeId);
        // Assert
        await Assert.ThrowsAsync<EdgeNotFoundException>(action);
    }
}
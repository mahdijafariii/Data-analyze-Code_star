using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Exception.GraphException.EdgeException;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using NSubstitute;

namespace TestProject.Services.GraphService.GraphServices.NodeAndEdgeInfo;

public class NodeAndEdgeInfoTests
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IGraphEdgeRepository _graphEdgeRepository;
    private readonly AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.NodeAndEdgeInfo _sut;

    public NodeAndEdgeInfoTests()
    {
        _graphNodeRepository = Substitute.For<IGraphNodeRepository>();
        _graphEdgeRepository = Substitute.For<IGraphEdgeRepository>();
        _sut = new AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.NodeAndEdgeInfo(_graphNodeRepository, _graphEdgeRepository);
    }

    [Fact]
    public async Task GetNodeInformationAsync_ShouldReturnNodeAttributes_WhenRoleIsNotDataAnalyst()
    {
        // Arrange
        var nodeId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var nodeAttributes = new List<NodeInformationDto>
        {
            new NodeInformationDto { Attribute = "Color", Value = "Red" },
            new NodeInformationDto { Attribute = "Size", Value = "Large" }
        };

        _graphNodeRepository.GetNodeAttributeValueAsync(nodeId).Returns(Task.FromResult(nodeAttributes.AsEnumerable()));

        // Act
        var result = await _sut.GetNodeInformationAsync(claimsPrincipal, nodeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Red", result["Color"]);
        Assert.Equal("Large", result["Size"]);
    }

    [Fact]
    public async Task GetNodeInformationAsync_ShouldReturnNodeAttributes_WhenRoleIsDataAnalystAndNodeIsAccessible()
    {
        // Arrange
        var nodeId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "dataanalyst"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var nodeAttributes = new List<NodeInformationDto>
        {
            new NodeInformationDto { Attribute = "Color", Value = "Blue" }
        };

        var usernameGuid = Guid.Parse(claimsPrincipal.FindFirstValue("id"));

        _graphNodeRepository.IsNodeAccessibleByUser(usernameGuid, nodeId).Returns(Task.FromResult(true));
        _graphNodeRepository.GetNodeAttributeValueAsync(nodeId).Returns(Task.FromResult(nodeAttributes.AsEnumerable()));

        // Act
        var result = await _sut.GetNodeInformationAsync(claimsPrincipal, nodeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Blue", result["Color"]);
    }

    [Fact]
    public async Task GetNodeInformationAsync_ShouldThrowNodeNotFoundException_WhenNoAttributesFound()
    {
        // Arrange
        var nodeId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        _graphNodeRepository.GetNodeAttributeValueAsync(nodeId).Returns(Task.FromResult(Enumerable.Empty<NodeInformationDto>()));

        // Act
        Exception exception = await Record.ExceptionAsync(() => _sut.GetNodeInformationAsync(claimsPrincipal, nodeId));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<NodeNotFoundException>(exception);
    }

    [Fact]
    public async Task GetEdgeInformationAsync_ShouldReturnEdgeAttributes_WhenRoleIsNotDataAnalyst()
    {
        // Arrange
        var edgeId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var edgeAttributes = new List<EdgeInformationDto>
        {
            new EdgeInformationDto { Attribute = "Weight", Value = "5" }
        };

        _graphEdgeRepository.GetEdgeAttributeValues(edgeId).Returns(Task.FromResult(edgeAttributes.AsEnumerable()));

        // Act
        var result = await _sut.GetEdgeInformationAsync(claimsPrincipal, edgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("5", result["Weight"]);
    }

    [Fact]
    public async Task GetEdgeInformationAsync_ShouldReturnEdgeAttributes_WhenRoleIsDataAnalystAndEdgeIsAccessible()
    {
        // Arrange
        var edgeId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "dataanalyst"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        var edgeAttributes = new List<EdgeInformationDto>
        {
            new EdgeInformationDto { Attribute = "Type", Value = "Direct" }
        };

        var username = claimsPrincipal.FindFirstValue("id");

        _graphEdgeRepository.IsEdgeAccessibleByUser(username, edgeId).Returns(Task.FromResult(true));
        _graphEdgeRepository.GetEdgeAttributeValues(edgeId).Returns(Task.FromResult(edgeAttributes.AsEnumerable()));

        // Act
        var result = await _sut.GetEdgeInformationAsync(claimsPrincipal, edgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Direct", result["Type"]);
    }

    [Fact]
    public async Task GetEdgeInformationAsync_ShouldThrowEdgeNotFoundException_WhenNoAttributesFound()
    {
        // Arrange
        var edgeId = Guid.NewGuid();
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "admin"),
            new Claim("id", Guid.NewGuid().ToString())
        }));

        _graphEdgeRepository.GetEdgeAttributeValues(edgeId).Returns(Task.FromResult(Enumerable.Empty<EdgeInformationDto>()));

        // Act
        Exception exception = await Record.ExceptionAsync(() => _sut.GetEdgeInformationAsync(claimsPrincipal, edgeId));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<EdgeNotFoundException>(exception);
    }
}

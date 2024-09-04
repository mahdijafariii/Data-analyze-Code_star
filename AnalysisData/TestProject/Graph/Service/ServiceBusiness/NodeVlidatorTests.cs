using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Service.ServiceBusiness;

namespace TestProject.Graph.Service.ServiceBusiness;

public class NodeValidatorTests
{
    private readonly NodeValidator _nodeValidator;

    public NodeValidatorTests()
    {
        _nodeValidator = new NodeValidator();
    }

    [Fact]
    public async Task ValidateNodesExistenceAsync_ShouldThrowException_WhenFromNodeIsNull()
    {
        // Arrange
        EntityNode? fromNode = null;
        var toNode = new EntityNode { Id = 2, Name = "ToNode" };
        var entityFrom = "FromNodeId";
        var entityTo = "ToNodeId";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NodeNotFoundInEntityEdgeException>(() =>
            _nodeValidator.ValidateNodesExistenceAsync(fromNode, toNode, entityFrom, entityTo));

        Assert.Contains(entityFrom, exception.Message);
        Assert.DoesNotContain(entityTo, exception.Message);
    }

    [Fact]
    public async Task ValidateNodesExistenceAsync_ShouldThrowException_WhenToNodeIsNull()
    {
        // Arrange
        var fromNode = new EntityNode { Id = 1, Name = "FromNode" };
        EntityNode? toNode = null;
        var entityFrom = "FromNodeId";
        var entityTo = "ToNodeId";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NodeNotFoundInEntityEdgeException>(() =>
            _nodeValidator.ValidateNodesExistenceAsync(fromNode, toNode, entityFrom, entityTo));

        Assert.Contains(entityTo, exception.Message);
        Assert.DoesNotContain(entityFrom, exception.Message);
    }

    [Fact]
    public async Task ValidateNodesExistenceAsync_ShouldThrowException_WhenBothNodesAreNull()
    {
        // Arrange
        EntityNode? fromNode = null;
        EntityNode? toNode = null;
        var entityFrom = "FromNodeId";
        var entityTo = "ToNodeId";

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NodeNotFoundInEntityEdgeException>(() =>
            _nodeValidator.ValidateNodesExistenceAsync(fromNode, toNode, entityFrom, entityTo));

        Assert.Contains(entityFrom, exception.Message);
        Assert.Contains(entityTo, exception.Message);
    }

    [Fact]
    public async Task ValidateNodesExistenceAsync_ShouldNotThrowException_WhenBothNodesExist()
    {
        // Arrange
        var fromNode = new EntityNode { Id = 1, Name = "FromNode" };
        var toNode = new EntityNode { Id = 2, Name = "ToNode" };
        var entityFrom = "FromNodeId";
        var entityTo = "ToNodeId";

        // Act
        await _nodeValidator.ValidateNodesExistenceAsync(fromNode, toNode, entityFrom, entityTo);
        
    }
}
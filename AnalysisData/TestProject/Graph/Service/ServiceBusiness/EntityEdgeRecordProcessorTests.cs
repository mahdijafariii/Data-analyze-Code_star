using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EntityEdgeRecordProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly INodeValidator _nodeValidator;
    private readonly ICsvReader _csvReader;
    private readonly EntityEdgeRecordProcessor _processor;

    public EntityEdgeRecordProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _entityEdgeRepository = Substitute.For<IEntityEdgeRepository>();
        _nodeValidator = Substitute.For<INodeValidator>();
        _csvReader = Substitute.For<ICsvReader>();
        _processor = new EntityEdgeRecordProcessor(_entityNodeRepository, _entityEdgeRepository, _nodeValidator, _csvReader);
    }

    [Fact]
    public async Task ProcessEntityEdgesAsync_CreatesAndInsertsEdges()
    {
        // Arrange
        _csvReader.Read().Returns(true, true, false);
        _csvReader.GetField("From").Returns("Node1", "Node2");
        _csvReader.GetField("To").Returns("Node3", "Node4");

        var node1 = new EntityNode { Id = 1 };
        var node2 = new EntityNode { Id = 2 };
        var node3 = new EntityNode { Id = 3 };
        var node4 = new EntityNode { Id = 4 };

        _entityNodeRepository.GetByNameAsync("Node1").Returns(node1);
        _entityNodeRepository.GetByNameAsync("Node2").Returns(node2);
        _entityNodeRepository.GetByNameAsync("Node3").Returns(node3);
        _entityNodeRepository.GetByNameAsync("Node4").Returns(node4);

        _nodeValidator.ValidateNodesExistenceAsync(Arg.Any<EntityNode>(), Arg.Any<EntityNode>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _processor.ProcessEntityEdgesAsync("From", "To");

        // Assert
        Assert.Equal(2, result.Count());
        await _entityEdgeRepository.Received(1).AddRangeAsync(Arg.Any<IEnumerable<EntityEdge>>());
    }

    /*[Fact]
    public async Task CreateEntityEdgeAsync_ThrowsExceptionIfNodeNotFound()
    {
        // Arrange
        _csvReader.Read().Returns(true, false);
        _csvReader.GetField("From").Returns("NonExistentNode");
        _csvReader.GetField("To").Returns("ExistingNode");

        _entityNodeRepository.GetByNameAsync("NonExistentNode").Returns((EntityNode)null);
        _entityNodeRepository.GetByNameAsync("ExistingNode").Returns(new EntityNode { Id = 1 });

        // Act & Assert
        await _processor.ProcessEntityEdgesAsync("From", "To");
    }*/
}

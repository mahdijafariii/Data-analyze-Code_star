using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.EdgeManager;

public class EdgeDataProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    private readonly EdgeDataProcessor _sut;
    private readonly ICsvReaderProcessor _csvReaderProcessor;

    public EdgeDataProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _entityEdgeRepository = Substitute.For<IEntityEdgeRepository>();
        _valueEdgeRepository = Substitute.For<IValueEdgeRepository>();
        _csvReaderProcessor = Substitute.For<ICsvReaderProcessor>();
        _sut = new EdgeDataProcessor(
            _entityNodeRepository,
            _entityEdgeRepository,
            _valueEdgeRepository,
            2 
        );
    }

    [Fact]
    public async Task ProcessEdgesAsync_Should_ProcessBatch_AndInsertEntities_When_ValidDataProvided()
    {
        // Arrange
        var attributeEdges = new List<AttributeEdge>
        {
            new AttributeEdge { Id = Guid.NewGuid(), Name = "Weight" },
            new AttributeEdge { Id = Guid.NewGuid(), Name = "Capacity" }
        };

        _csvReaderProcessor.Read().Returns(true, true, false); 
        _csvReaderProcessor.GetField("from").Returns("Node1");
        _csvReaderProcessor.GetField("to").Returns("Node3");
        _csvReaderProcessor.GetField("Weight").Returns("10", "15");
        _csvReaderProcessor.GetField("Capacity").Returns("100", "150");

        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "Node1" };
        var node3 = new EntityNode { Id = Guid.NewGuid(), Name = "Node3" };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "Node2" };
        var node4 = new EntityNode { Id = Guid.NewGuid(), Name = "Node4" };

        _entityNodeRepository.GetByNameAsync("Node1").Returns(Task.FromResult(node1));
        _entityNodeRepository.GetByNameAsync("Node3").Returns(Task.FromResult(node3));
        _entityNodeRepository.GetByNameAsync("Node2").Returns(Task.FromResult(node2));
        _entityNodeRepository.GetByNameAsync("Node4").Returns(Task.FromResult(node4));

        // Act
        await _sut.ProcessEdgesAsync(_csvReaderProcessor, attributeEdges, "from", "to");

        // Assert
        await _entityEdgeRepository.Received(1).AddRangeAsync(Arg.Any<List<EntityEdge>>());
        await _valueEdgeRepository.Received(1).AddRangeAsync(Arg.Any<List<ValueEdge>>());
    }
    
    [Fact]
    public async Task ProcessEdgesAsync_Should_Ignore_FromAndToFields_InValueEdge_When_InsertingAttributes()
    {
        // Arrange
        var attributeEdges = new List<AttributeEdge>
        {
            new AttributeEdge { Id = Guid.NewGuid(), Name = "from" }, 
            new AttributeEdge { Id = Guid.NewGuid(), Name = "to" },   
            new AttributeEdge { Id = Guid.NewGuid(), Name = "Weight" }
        };

        _csvReaderProcessor.Read().Returns(true, false); 
        _csvReaderProcessor.GetField("from").Returns("Node1");
        _csvReaderProcessor.GetField("to").Returns("Node3");
        _csvReaderProcessor.GetField("Weight").Returns("10");

        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "Node1" };
        var node3 = new EntityNode { Id = Guid.NewGuid(), Name = "Node3" };

        _entityNodeRepository.GetByNameAsync("Node1").Returns(Task.FromResult(node1));
        _entityNodeRepository.GetByNameAsync("Node3").Returns(Task.FromResult(node3));

        // Act
        await _sut.ProcessEdgesAsync(_csvReaderProcessor, attributeEdges, "from", "to");

        // Assert
        await _valueEdgeRepository.Received(1).AddRangeAsync(Arg.Is<List<ValueEdge>>(v =>
            v.All(ve => ve.AttributeId != attributeEdges[0].Id && ve.AttributeId != attributeEdges[1].Id)
        ));
    }
}

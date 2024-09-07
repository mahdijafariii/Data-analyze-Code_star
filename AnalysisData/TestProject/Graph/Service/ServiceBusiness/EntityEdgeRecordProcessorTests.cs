using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EntityEdgeRecordProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly ICsvReader _csvReader;
    private readonly EntityEdgeRecordProcessor _sut;

    public EntityEdgeRecordProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _entityEdgeRepository = Substitute.For<IEntityEdgeRepository>();
        _csvReader = Substitute.For<ICsvReader>();

        _sut = new EntityEdgeRecordProcessor(_entityNodeRepository, _entityEdgeRepository);
    }

    [Fact]
    public async Task ProcessEntityEdgesAsync_ShouldCreateEntityEdges_WhenCsvHasValidRecords()
    {
        // Arrange
        _csvReader.Read().Returns(true, false);
        _csvReader.GetField("from").Returns("Node1");
        _csvReader.GetField("to").Returns("Node2");

        _entityNodeRepository.GetByNameAsync("Node1").Returns(new EntityNode { Id = 1, Name = "Node1" });
        _entityNodeRepository.GetByNameAsync("Node2").Returns(new EntityNode { Id = 2, Name = "Node2" });

        // Act
        var result = await _sut.ProcessEntityEdgesAsync(_csvReader, "from", "to");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var entityEdge = result.First();
        Assert.Equal(1, entityEdge.EntityIDSource);
        Assert.Equal(2, entityEdge.EntityIDTarget);
        
        await _entityEdgeRepository.Received(1).AddRangeAsync(Arg.Is<IEnumerable<EntityEdge>>(edges => edges.Count() == 1));
    }

    [Fact]
    public async Task ProcessEntityEdgesAsync_ShouldThrowException_WhenNodeNotFound()
    {
        // Arrange
        _csvReader.Read().Returns(true, false); 
        _csvReader.GetField("from").Returns("Node1");
        _csvReader.GetField("to").Returns("Node2");

        _entityNodeRepository.GetByNameAsync("Node1").Returns((EntityNode)null); 

        // Act & Assert
        await Assert.ThrowsAsync<NodeNotFoundInEntityEdgeException>(async () =>
            await _sut.ProcessEntityEdgesAsync(_csvReader, "from", "to")
        );
    }

    [Fact]
    public async Task ProcessEntityEdgesAsync_ShouldSkipEmptyCsv()
    {
        // Arrange
        _csvReader.Read().Returns(false);

        // Act
        var result = await _sut.ProcessEntityEdgesAsync(_csvReader, "from", "to");

        // Assert
        Assert.Empty(result);
        await _entityEdgeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<EntityEdge>>());
    }
}
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EdgeDataProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly ICsvReaderProcessor _csvReaderProcessor;
    private readonly EdgeDataProcessor _sut;

    public EdgeDataProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _entityEdgeRepository = Substitute.For<IEntityEdgeRepository>();
        _csvReaderProcessor = Substitute.For<ICsvReaderProcessor>();

        _sut = new EdgeDataProcessor(_entityNodeRepository, _entityEdgeRepository);
    }

    [Fact]
    public async Task ProcessEntityEdgesAsync_ShouldCreateEntityEdges_WhenCsvHasValidRecords()
    {
        // Arrange
        _csvReaderProcessor.Read().Returns(true, false);
        _csvReaderProcessor.GetField("from").Returns("Node1");
        _csvReaderProcessor.GetField("to").Returns("Node2");

        _entityNodeRepository.GetByNameAsync("Node1").Returns(new EntityNode { Id = 1, Name = "Node1" });
        _entityNodeRepository.GetByNameAsync("Node2").Returns(new EntityNode { Id = 2, Name = "Node2" });

        // Act
        var result = await _sut.ProcessEntityEdgesAsync(_csvReaderProcessor, "from", "to");

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
        _csvReaderProcessor.Read().Returns(true, false); 
        _csvReaderProcessor.GetField("from").Returns("Node1");
        _csvReaderProcessor.GetField("to").Returns("Node2");

        _entityNodeRepository.GetByNameAsync("Node1").Returns((EntityNode)null); 

        // Act & Assert
        await Assert.ThrowsAsync<NodeNotFoundInEntityEdgeException>(async () =>
            await _sut.ProcessEntityEdgesAsync(_csvReaderProcessor, "from", "to")
        );
    }

    [Fact]
    public async Task ProcessEntityEdgesAsync_ShouldSkipEmptyCsv()
    {
        // Arrange
        _csvReaderProcessor.Read().Returns(false);

        // Act
        var result = await _sut.ProcessEntityEdgesAsync(_csvReaderProcessor, "from", "to");

        // Assert
        Assert.Empty(result);
        await _entityEdgeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<EntityEdge>>());
    }
}
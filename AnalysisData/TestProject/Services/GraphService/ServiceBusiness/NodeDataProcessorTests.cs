using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class NodeDataProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly ICsvReaderProcessor _csvReaderProcessor;
    private readonly NodeDataProcessor _sut;

    private const int BatchSize = 3;

    public NodeDataProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _csvReaderProcessor = Substitute.For<ICsvReaderProcessor>();
        
        _sut = new NodeDataProcessor(_entityNodeRepository, BatchSize);
    }
    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldSkipRecords_WhenFieldIsEmpty()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReaderProcessor.Read().Returns(true, true, true, false); 
        _csvReaderProcessor.GetField("Id").Returns("Node1", "", "Node3");

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReaderProcessor, headers, "Id", 1);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldInsertRemainingRecords_WhenLessThanBatchSize()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReaderProcessor.Read().Returns(true, true, false);
        _csvReaderProcessor.GetField("Id").Returns("Node1", "Node2");

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReaderProcessor, headers, "Id", 1);

        // Assert
        Assert.Equal(2, result.Count());
        await _entityNodeRepository.Received(1).AddRangeAsync(Arg.Is<IEnumerable<EntityNode>>(nodes => nodes.Count() == 2));
    }

    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldProcessAllRecords_WhenCsvHasMultipleBatches()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReaderProcessor.Read().Returns(true, true, true, true, true, false); 
        _csvReaderProcessor.GetField("Id").Returns("Node1", "Node2", "Node3", "Node4", "Node5");

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReaderProcessor, headers, "Id", 1);

        // Assert
        Assert.Equal(5, result.Count());
        
        await _entityNodeRepository.Received(2).AddRangeAsync(Arg.Any<IEnumerable<EntityNode>>());
    }

    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldNotInsert_WhenCsvIsEmpty()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReaderProcessor.Read().Returns(false); 

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReaderProcessor, headers, "Id", 1);

        // Assert
        Assert.Empty(result);
        await _entityNodeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<EntityNode>>());
    }
}
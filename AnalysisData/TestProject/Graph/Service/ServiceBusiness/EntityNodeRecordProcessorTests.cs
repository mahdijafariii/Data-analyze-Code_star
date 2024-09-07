using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EntityNodeRecordProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly ICsvReader _csvReader;
    private readonly EntityNodeRecordProcessor _sut;

    private const int BatchSize = 3;

    public EntityNodeRecordProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _csvReader = Substitute.For<ICsvReader>();
        
        _sut = new EntityNodeRecordProcessor(_entityNodeRepository, BatchSize);
    }
    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldSkipRecords_WhenFieldIsEmpty()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReader.Read().Returns(true, true, true, false); 
        _csvReader.GetField("Id").Returns("Node1", "", "Node3");

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReader, headers, "Id", 1);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldInsertRemainingRecords_WhenLessThanBatchSize()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReader.Read().Returns(true, true, false);
        _csvReader.GetField("Id").Returns("Node1", "Node2");

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReader, headers, "Id", 1);

        // Assert
        Assert.Equal(2, result.Count());
        await _entityNodeRepository.Received(1).AddRangeAsync(Arg.Is<IEnumerable<EntityNode>>(nodes => nodes.Count() == 2));
    }

    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldProcessAllRecords_WhenCsvHasMultipleBatches()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReader.Read().Returns(true, true, true, true, true, false); 
        _csvReader.GetField("Id").Returns("Node1", "Node2", "Node3", "Node4", "Node5");

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReader, headers, "Id", 1);

        // Assert
        Assert.Equal(5, result.Count());
        
        await _entityNodeRepository.Received(2).AddRangeAsync(Arg.Any<IEnumerable<EntityNode>>());
    }

    [Fact]
    public async Task ProcessEntityNodesAsync_ShouldNotInsert_WhenCsvIsEmpty()
    {
        // Arrange
        var headers = new List<string> { "Id" };
        _csvReader.Read().Returns(false); 

        // Act
        var result = await _sut.ProcessEntityNodesAsync(_csvReader, headers, "Id", 1);

        // Assert
        Assert.Empty(result);
        await _entityNodeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<EntityNode>>());
    }
}
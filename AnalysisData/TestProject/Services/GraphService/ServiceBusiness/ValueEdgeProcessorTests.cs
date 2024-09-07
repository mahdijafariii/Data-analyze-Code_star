using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class ValueEdgeProcessorTests
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    private readonly ValueEdgeProcessor _sut;
    private readonly ICsvReaderProcessor _csvReaderProcessor;

    public ValueEdgeProcessorTests()
    {
        _attributeEdgeRepository = Substitute.For<IAttributeEdgeRepository>();
        _valueEdgeRepository = Substitute.For<IValueEdgeRepository>();
        _csvReaderProcessor = Substitute.For<ICsvReaderProcessor>();

        _sut = new ValueEdgeProcessor(
            _attributeEdgeRepository,
            _valueEdgeRepository,
            3 
        );
    }

    [Fact]
    public async Task ProcessEntityEdgeValuesAsync_ShouldProcessBatches_WhenRecordsExceedBatchSize()
    {
        // Arrange
        var entityEdges = new List<EntityEdge>
        {
            new EntityEdge { Id = 1 },
            new EntityEdge { Id = 2 }
        };

        var headers = new List<string> { "Attribute1", "Attribute2", "From", "To" };

        _csvReaderProcessor.Read().Returns(true, true, true, false);
        _csvReaderProcessor.GetField("Attribute1").Returns("Value1");
        _csvReaderProcessor.GetField("Attribute2").Returns("Value2");

        _attributeEdgeRepository.GetByNameAsync("Attribute1").Returns(Task.FromResult(new AttributeEdge { Id = 1 }));
        _attributeEdgeRepository.GetByNameAsync("Attribute2").Returns(Task.FromResult(new AttributeEdge { Id = 2 }));

        // Act
        await _sut.ProcessEntityEdgeValuesAsync(
            _csvReaderProcessor,
            headers,
            "From",
            "To",
            entityEdges
        );

        // Assert
        await _valueEdgeRepository.Received(2).AddRangeAsync(Arg.Any<List<ValueEdge>>());
    }

    [Fact]
    public async Task ProcessEntityEdgeValuesAsync_ShouldNotAddRange_WhenCsvIsEmpty()
    {
        // Arrange
        var entityEdges = new List<EntityEdge>();
        var headers = new List<string>();

        _csvReaderProcessor.Read().Returns(false);

        // Act
        await _sut.ProcessEntityEdgeValuesAsync(
            _csvReaderProcessor,
            headers,
            "From",
            "To",
            entityEdges
        );

        // Assert
        await _valueEdgeRepository.DidNotReceive().AddRangeAsync(Arg.Any<List<ValueEdge>>());
    }

    [Fact]
    public async Task ProcessEntityEdgeValuesAsync_ShouldProcessRemainingRecords_WhenCsvHasRemainingRecords()
    {
        // Arrange
        var entityEdges = new List<EntityEdge>
        {
            new EntityEdge { Id = 1 }
        };

        var headers = new List<string> { "Attribute1", "Attribute2" };

        _csvReaderProcessor.Read().Returns(true, false); 
        _csvReaderProcessor.GetField("Attribute1").Returns("Value1");

        _attributeEdgeRepository.GetByNameAsync("Attribute1").Returns(Task.FromResult(new AttributeEdge { Id = 1 }));

        // Act
        await _sut.ProcessEntityEdgeValuesAsync(
            _csvReaderProcessor,
            headers,
            "From",
            "To",
            entityEdges
        );

        // Assert
        await _valueEdgeRepository.Received(1).AddRangeAsync(Arg.Is<List<ValueEdge>>(ve => ve.Count == 1));
    }
}
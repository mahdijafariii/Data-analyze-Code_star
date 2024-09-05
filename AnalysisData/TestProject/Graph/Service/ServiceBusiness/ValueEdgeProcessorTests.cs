using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class ValueEdgeProcessorTests
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly IValueEdgeRepository _valueEdgeRepository;
    private readonly ValueEdgeProcessor _sut;
    private readonly ICsvReader _csvReader;

    public ValueEdgeProcessorTests()
    {
        _attributeEdgeRepository = Substitute.For<IAttributeEdgeRepository>();
        _valueEdgeRepository = Substitute.For<IValueEdgeRepository>();
        _csvReader = Substitute.For<ICsvReader>();

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

        _csvReader.Read().Returns(true, true, true, false);
        _csvReader.GetField("Attribute1").Returns("Value1");
        _csvReader.GetField("Attribute2").Returns("Value2");

        _attributeEdgeRepository.GetByNameAsync("Attribute1").Returns(Task.FromResult(new AttributeEdge { Id = 1 }));
        _attributeEdgeRepository.GetByNameAsync("Attribute2").Returns(Task.FromResult(new AttributeEdge { Id = 2 }));

        // Act
        await _sut.ProcessEntityEdgeValuesAsync(
            _csvReader,
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

        _csvReader.Read().Returns(false);

        // Act
        await _sut.ProcessEntityEdgeValuesAsync(
            _csvReader,
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

        _csvReader.Read().Returns(true, false); 
        _csvReader.GetField("Attribute1").Returns("Value1");

        _attributeEdgeRepository.GetByNameAsync("Attribute1").Returns(Task.FromResult(new AttributeEdge { Id = 1 }));

        // Act
        await _sut.ProcessEntityEdgeValuesAsync(
            _csvReader,
            headers,
            "From",
            "To",
            entityEdges
        );

        // Assert
        await _valueEdgeRepository.Received(1).AddRangeAsync(Arg.Is<List<ValueEdge>>(ve => ve.Count == 1));
    }
}
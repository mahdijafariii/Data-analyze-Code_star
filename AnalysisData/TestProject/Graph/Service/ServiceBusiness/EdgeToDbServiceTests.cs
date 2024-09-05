using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EdgeToDbServiceTests
{
    private readonly IValueEdgeProcessor _valueEdgeProcessor;
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderService _csvReaderService;
    private readonly EdgeToDbService _sut;

    public EdgeToDbServiceTests()
    {
        _valueEdgeProcessor = Substitute.For<IValueEdgeProcessor>();
        _entityEdgeRecordProcessor = Substitute.For<IEntityEdgeRecordProcessor>();
        _fromToProcessor = Substitute.For<IFromToProcessor>();
        _csvReaderService = Substitute.For<ICsvReaderService>();

        _sut = new EdgeToDbService(
            _fromToProcessor,
            _csvReaderService,
            _entityEdgeRecordProcessor,
            _valueEdgeProcessor
        );
    }

    [Fact]
    public async Task ProcessCsvFileAsync_ShouldProcessCsvCorrectly()
    {
        // Arrange
        var file = Substitute.For<IFormFile>(); 
        var csvReader = Substitute.For<ICsvReader>();

        var headers = new List<string> { "From", "To", "OtherHeader" };
        var entityEdges = new List<EntityEdge>
        {
            new EntityEdge { Id = 1 },
            new EntityEdge { Id = 2 }
        };

        _csvReaderService.CreateCsvReader(file).Returns(csvReader);

        _csvReaderService.ReadHeaders(csvReader, Arg.Any<List<string>>())
            .Returns(headers);
        
        _entityEdgeRecordProcessor
            .ProcessEntityEdgesAsync(Arg.Any<ICsvReader>(), Arg.Any<string>(), Arg.Any<string>())
            .Returns(Task.FromResult((IEnumerable<EntityEdge>)entityEdges));

        // Act
        await _sut.ProcessCsvFileAsync(file, "From", "To");

        // Assert
        await _fromToProcessor.Received(1).ProcessFromToAsync(headers, "From", "To");
        await _entityEdgeRecordProcessor.Received(1).ProcessEntityEdgesAsync(csvReader, "From", "To");
        await _valueEdgeProcessor.Received(1)
            .ProcessEntityEdgeValuesAsync(csvReader, headers, "From", "To", entityEdges);
    }
}
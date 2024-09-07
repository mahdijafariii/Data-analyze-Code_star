using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class EdgeToDbServiceTests
{
    private readonly IValueEdgeProcessor _valueEdgeProcessor;
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderManager _csvReaderManager;
    private readonly EdgeToDbProcessor _sut;

    public EdgeToDbServiceTests()
    {
        _valueEdgeProcessor = Substitute.For<IValueEdgeProcessor>();
        _entityEdgeRecordProcessor = Substitute.For<IEntityEdgeRecordProcessor>();
        _fromToProcessor = Substitute.For<IFromToProcessor>();
        _csvReaderManager = Substitute.For<ICsvReaderManager>();

        _sut = new EdgeToDbProcessor(
            _fromToProcessor,
            _csvReaderManager,
            _entityEdgeRecordProcessor,
            _valueEdgeProcessor
        );
    }

    [Fact]
    public async Task ProcessCsvFileAsync_ShouldProcessCsvCorrectly()
    {
        // Arrange
        var file = Substitute.For<IFormFile>(); 
        var csvReader = Substitute.For<ICsvReaderProcessor>();

        var headers = new List<string> { "From", "To", "OtherHeader" };
        var entityEdges = new List<EntityEdge>
        {
            new EntityEdge { Id = 1 },
            new EntityEdge { Id = 2 }
        };

        _csvReaderManager.CreateCsvReader(file).Returns(csvReader);

        _csvReaderManager.ReadHeaders(csvReader, Arg.Any<List<string>>())
            .Returns(headers);
        
        _entityEdgeRecordProcessor
            .ProcessEntityEdgesAsync(Arg.Any<ICsvReaderProcessor>(), Arg.Any<string>(), Arg.Any<string>())
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
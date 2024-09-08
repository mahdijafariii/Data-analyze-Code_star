using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.EdgeManager;

public class EdgeToDbProcessorTests
{
    private readonly IEntityEdgeRecordProcessor _entityEdgeRecordProcessor;
    private readonly IFromToProcessor _fromToProcessor;
    private readonly ICsvReaderManager _csvReaderManager;
    private readonly EdgeToDbProcessor _sut;

    public EdgeToDbProcessorTests()
    {
        _entityEdgeRecordProcessor = Substitute.For<IEntityEdgeRecordProcessor>();
        _fromToProcessor = Substitute.For<IFromToProcessor>();
        _csvReaderManager = Substitute.For<ICsvReaderManager>();

        _sut = new EdgeToDbProcessor(
            _fromToProcessor,
            _csvReaderManager,
            _entityEdgeRecordProcessor
        );
    }

    [Fact]
    public async Task ProcessCsvFileAsync_Should_ProcessCsvCorrectly_When_FileAndHeadersAreValid()
    {
        // Arrange
        var file = Substitute.For<IFormFile>(); 
        var csvReader = Substitute.For<ICsvReaderProcessor>();

        var headers = new List<string> { "From", "To", "OtherHeader" };
    
        var headersWithId = new List<AttributeEdge>
        {
            new AttributeEdge { Id = Guid.NewGuid(), Name = "OtherHeader" },
        };
    
        _csvReaderManager.CreateCsvReader(file).Returns(csvReader);
        _csvReaderManager.ReadHeaders(csvReader, Arg.Any<List<string>>())
            .Returns(headers);

        _fromToProcessor.ProcessFromToAsync(headers, "From", "To")
            .Returns(Task.FromResult<IEnumerable<AttributeEdge>>(headersWithId)); 
    
        _entityEdgeRecordProcessor
            .ProcessEdgesAsync(csvReader, headersWithId, "From", "To")
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessCsvFileAsync(file, "From", "To");

        // Assert
        await _fromToProcessor.Received(1).ProcessFromToAsync(headers, "From", "To");
        await _entityEdgeRecordProcessor.Received(1).ProcessEdgesAsync(csvReader, headersWithId, "From", "To");
    }
}

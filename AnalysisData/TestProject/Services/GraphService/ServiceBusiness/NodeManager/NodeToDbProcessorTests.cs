using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.NodeManager;

public class NodeToDbProcessorTests
{
    private readonly ICsvReaderManager _csvReaderManager;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;
    private readonly NodeToDbProcessor _sut;

    public NodeToDbProcessorTests()
    {
        _csvReaderManager = Substitute.For<ICsvReaderManager>();
        _headerProcessor = Substitute.For<IHeaderProcessor>();
        _nodeRecordProcessor = Substitute.For<INodeRecordProcessor>();
        _sut = new NodeToDbProcessor(
            _csvReaderManager,
            _headerProcessor,
            _nodeRecordProcessor
        );
    }

    [Fact]
    public async Task ProcessCsvFileAsync_Should_ProcessFile_AndPassCorrectHeaders_When_ValidFileProvided()
    {
        // Arrange
        var formFile = Substitute.For<IFormFile>();
        var csvReader = Substitute.For<ICsvReaderProcessor>();
        var id = "Id";
        var fileId = 1;
        
        var headers = new List<string> { "Id", "Name", "Value" };
        
        var processedHeaders = new List<AttributeNode>()
        {
            new AttributeNode { Id = Guid.NewGuid(), Name = "Id" },
            new AttributeNode { Id = Guid.NewGuid(), Name = "Name" },
            new AttributeNode { Id = Guid.NewGuid(), Name = "Value" }
        };
        
        _csvReaderManager.CreateCsvReader(formFile).Returns(csvReader);
        _csvReaderManager.ReadHeaders(csvReader, Arg.Is<List<string>>(list => list.Contains(id)))
            .Returns(headers);
        
        _headerProcessor.ProcessHeadersAsync(headers, id).Returns(Task.FromResult<IEnumerable<AttributeNode>>(processedHeaders));

        // Act
        await _sut.ProcessCsvFileAsync(formFile, id, fileId);

        // Assert
        _csvReaderManager.Received(1).CreateCsvReader(formFile);
        _csvReaderManager.Received(1).ReadHeaders(csvReader, Arg.Is<List<string>>(list => list.Contains(id)));
        await _headerProcessor.Received(1).ProcessHeadersAsync(headers, id);
        await _nodeRecordProcessor.Received(1).ProcessNodesAsync(csvReader, processedHeaders, id, fileId);
    }

}
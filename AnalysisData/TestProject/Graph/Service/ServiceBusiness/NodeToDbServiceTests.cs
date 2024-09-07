using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager;
using AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class NodeToDbServiceTests
{
    private readonly NodeToDbProcessor _sut;
    private readonly ICsvReaderManager _csvReaderManager;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;
    private readonly IValueNodeProcessor _valueNodeProcessor;

    public NodeToDbServiceTests()
    {
        _csvReaderManager = Substitute.For<ICsvReaderManager>();
        _headerProcessor = Substitute.For<IHeaderProcessor>();
        _nodeRecordProcessor = Substitute.For<INodeRecordProcessor>();
        _valueNodeProcessor = Substitute.For<IValueNodeProcessor>();

        _sut = new NodeToDbProcessor(
            _csvReaderManager,
            _headerProcessor,
            _nodeRecordProcessor,
            _valueNodeProcessor
        );
    }

    [Fact]
    public async Task ProcessCsvFileAsync_ShouldProcessFileCorrectly_WhenFileIsValid()
    {
        // Arrange
        var mockFile = Substitute.For<IFormFile>();
        var mockCsvReader = Substitute.For<ICsvReaderProcessor>();
        var headers = new List<string> { "Header1", "Header2" };
        var entityNodes = new List<EntityNode> { new EntityNode { Name = "Node1" } };
        
        _csvReaderManager.CreateCsvReader(mockFile).Returns(mockCsvReader);
        _csvReaderManager.ReadHeaders(mockCsvReader, Arg.Any<List<string>>()).Returns(headers);
        _nodeRecordProcessor.ProcessEntityNodesAsync(mockCsvReader, headers, "IdField", 1)
            .Returns(Task.FromResult((IEnumerable<EntityNode>)entityNodes));
        _valueNodeProcessor.ProcessValueNodesAsync(mockCsvReader, entityNodes, headers, "IdField")
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessCsvFileAsync(mockFile, "IdField", 1);

        // Assert
        _csvReaderManager.Received(3).CreateCsvReader(mockFile);
        
        _csvReaderManager.Received(3).ReadHeaders(mockCsvReader, Arg.Any<List<string>>());
        
        await _headerProcessor.Received(1).ProcessHeadersAsync(headers, "IdField");
        
        await _nodeRecordProcessor.Received(1).ProcessEntityNodesAsync(mockCsvReader, headers, "IdField", 1);
        
        await _valueNodeProcessor.Received(1).ProcessValueNodesAsync(mockCsvReader, entityNodes, headers, "IdField");
    }
}
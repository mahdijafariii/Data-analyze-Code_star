using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Service.ServiceBusiness;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class NodeToDbServiceTests
{
    private readonly NodeToDbService _sut;
    private readonly ICsvReaderService _csvReaderService;
    private readonly IHeaderProcessor _headerProcessor;
    private readonly INodeRecordProcessor _nodeRecordProcessor;
    private readonly IValueNodeProcessor _valueNodeProcessor;

    public NodeToDbServiceTests()
    {
        _csvReaderService = Substitute.For<ICsvReaderService>();
        _headerProcessor = Substitute.For<IHeaderProcessor>();
        _nodeRecordProcessor = Substitute.For<INodeRecordProcessor>();
        _valueNodeProcessor = Substitute.For<IValueNodeProcessor>();

        _sut = new NodeToDbService(
            _csvReaderService,
            _headerProcessor,
            _nodeRecordProcessor,
            _valueNodeProcessor
        );
    }

    [Fact]
    public async Task ProcessCsvFileAsync_ShouldProcessFileCorrectly()
    {
        // Arrange
        var mockFile = Substitute.For<IFormFile>();
        var mockCsvReader = Substitute.For<ICsvReader>();
        var headers = new List<string> { "Header1", "Header2" };
        var entityNodes = new List<EntityNode> { new EntityNode { Name = "Node1" } };
        
        _csvReaderService.CreateCsvReader(mockFile).Returns(mockCsvReader);
        _csvReaderService.ReadHeaders(mockCsvReader, Arg.Any<List<string>>()).Returns(headers);
        _nodeRecordProcessor.ProcessEntityNodesAsync(mockCsvReader, headers, "IdField", 1)
            .Returns(Task.FromResult((IEnumerable<EntityNode>)entityNodes));
        _valueNodeProcessor.ProcessValueNodesAsync(mockCsvReader, entityNodes, headers, "IdField")
            .Returns(Task.CompletedTask);

        // Act
        await _sut.ProcessCsvFileAsync(mockFile, "IdField", 1);

        // Assert
        _csvReaderService.Received(3).CreateCsvReader(mockFile);
        
        _csvReaderService.Received(3).ReadHeaders(mockCsvReader, Arg.Any<List<string>>());
        
        await _headerProcessor.Received(1).ProcessHeadersAsync(headers, "IdField");
        
        await _nodeRecordProcessor.Received(1).ProcessEntityNodesAsync(mockCsvReader, headers, "IdField", 1);
        
        await _valueNodeProcessor.Received(1).ProcessValueNodesAsync(mockCsvReader, entityNodes, headers, "IdField");
    }
}
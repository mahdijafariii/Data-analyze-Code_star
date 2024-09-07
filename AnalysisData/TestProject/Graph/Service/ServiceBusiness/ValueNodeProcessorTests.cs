using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class ValueNodeProcessorTests
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;
    private readonly ValueNodeProcessor _sut;
    
    public ValueNodeProcessorTests()
    {
        _attributeNodeRepository = Substitute.For<IAttributeNodeRepository>();
        _valueNodeRepository = Substitute.For<IValueNodeRepository>();
        _sut = new ValueNodeProcessor(
            _attributeNodeRepository,
            _valueNodeRepository,
            2 
        );
    }

    [Fact]
    public async Task ProcessValueNodesAsync_ShouldBatchAndProcessNodes_WhenCsvHasMultipleRecords()
    {
        // Arrange
        var entityNodes = new List<EntityNode>
        {
            new EntityNode { Id = 1, Name = "amir" },
            new EntityNode { Id = 2, Name = "reza" }
        };
        var headers = new List<string> { "Name", "age" };

        var csv = Substitute.For<ICsvReaderProcessor>();
        csv.Read().Returns(true, true, false); 
        csv.GetField("Name").Returns("amir", "reza");
        csv.GetField("age").Returns("14", "13");
        _attributeNodeRepository.GetByNameAsync("Name").Returns(Task.FromResult(new AttributeNode { Id = 1 }));
        _attributeNodeRepository.GetByNameAsync("age").Returns(Task.FromResult(new AttributeNode { Id = 2 }));

        // Act
        await _sut.ProcessValueNodesAsync(csv, entityNodes, headers, "Name");

        // Assert
        await _valueNodeRepository.Received(1).AddRangeAsync(Arg.Any<List<ValueNode>>()); 
    }

    [Fact]
    public async Task ProcessValueNodesAsync_ShouldNotAddNodes_WhenCsvIsEmpty()
    {
        // Arrange
        var entityNodes = new List<EntityNode>
        {
            new EntityNode { Id = 1, Name = "Node1" }
        };
        var headers = new List<string> { "Attribute1" };

        var csv = Substitute.For<ICsvReaderProcessor>();
        csv.Read().Returns(false); // No rows

        // Act
        await _sut.ProcessValueNodesAsync(csv, entityNodes, headers, "Id");

        // Assert
        await _valueNodeRepository.DidNotReceive().AddRangeAsync(Arg.Any<List<ValueNode>>());
    }
}
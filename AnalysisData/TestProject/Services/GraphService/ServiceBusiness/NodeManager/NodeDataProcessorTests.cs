using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.NodeManager;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.NodeManager;

public class NodeDataProcessorTests
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IValueNodeRepository _valueNodeRepository;
    private readonly NodeDataProcessor _sut; 
    private const int BatchSize = 1000; 

    public NodeDataProcessorTests()
    {
        _entityNodeRepository = Substitute.For<IEntityNodeRepository>();
        _valueNodeRepository = Substitute.For<IValueNodeRepository>();
        _sut = new NodeDataProcessor(_entityNodeRepository, _valueNodeRepository, BatchSize);
    }

    [Fact]
    public async Task ProcessNodesAsync_Should_Process_And_Insert_Entities_And_Values_When_ValidDataProvided()
    {
        // Arrange
        var csvReader = Substitute.For<ICsvReaderProcessor>();
        var id = "Id";
        var fileId = 1;

        
        csvReader.Read().Returns(true, true, false); 
        csvReader.GetField(id).Returns("Entity1", "Entity2"); 
        
        var headers = new List<AttributeNode>
        {
            new AttributeNode { Id = Guid.NewGuid(), Name = "Id" },
            new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute1" },
            new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute2" }
        };
        
        csvReader.GetField("Attribute1").Returns("Value1-1", "Value1-2");
        csvReader.GetField("Attribute2").Returns("Value2-1", "Value2-2");

        // Act
        await _sut.ProcessNodesAsync(csvReader, headers, id, fileId);

        // Assert
        await _entityNodeRepository.Received(1).AddRangeAsync(Arg.Is<List<EntityNode>>(nodes =>
            nodes.Count == 2 &&
            nodes[0].Name == "Entity1" &&
            nodes[1].Name == "Entity2"));

        await _valueNodeRepository.Received(1).AddRangeAsync(Arg.Is<List<ValueNode>>(values =>
            values.Count == 4 && 
            values[0].Value == "Value1-1" &&
            values[1].Value == "Value2-1" &&
            values[2].Value == "Value1-2" &&
            values[3].Value == "Value2-2"
        ));
    }
    
    
    [Fact]
    public async Task ProcessNodesAsync_Should_BatchInsert_When_NodeCount_ReachesBatchSize()
    {
        // Arrange
        var csvReader = Substitute.For<ICsvReaderProcessor>();
        var id = "Id";
        var fileId = 1;
        
        var rowCount = BatchSize + 1;
        csvReader.Read().Returns(_ => rowCount-- > 0, _ => false);
        csvReader.GetField(id).Returns("Entity" + Guid.NewGuid());

        
        var headers = new List<AttributeNode>
        {
            new AttributeNode { Id = Guid.NewGuid(), Name = "Id" }, 
            new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute1" }
        };

        csvReader.GetField("Attribute1").Returns("Value1");

        // Act
        await _sut.ProcessNodesAsync(csvReader, headers, id, fileId);

        // Assert
        await _entityNodeRepository.Received(1).AddRangeAsync(Arg.Any<List<EntityNode>>());
        await _valueNodeRepository.Received(1).AddRangeAsync(Arg.Any<List<ValueNode>>());
    }
    
}
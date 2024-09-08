using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.EdgeManager;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.EdgeManager;

public class FromToProcessorTests
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly FromToProcessor _sut;

    public FromToProcessorTests()
    {
        _attributeEdgeRepository = Substitute.For<IAttributeEdgeRepository>();
        _sut = new FromToProcessor(_attributeEdgeRepository);
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldReturnEmptyList_WhenHeadersContainOnlyFromAndTo()
    {
        // Arrange
        var headers = new List<string> { "from", "to" };
        var from = "from";
        var to = "to";

        // Act
        var result = await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        Assert.Empty(result);
        await _attributeEdgeRepository.DidNotReceive().GetByNamesAsync(Arg.Any<IEnumerable<string>>());
        await _attributeEdgeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<AttributeEdge>>());
    }
    
    [Fact]
    public async Task ProcessFromToAsync_CreatesNewEdges_WhenHeadersDoNotExistInRepository()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "NewHeader" };
        var from = "Header1";
        var to = "Header2";

        var existingEdges = new List<AttributeEdge>
        {
            new AttributeEdge { Id = Guid.NewGuid(), Name = "Header1" },
            new AttributeEdge { Id = Guid.NewGuid(), Name = "Header2" }
        };

        _attributeEdgeRepository.GetByNamesAsync(Arg.Any<IEnumerable<string>>())
            .Returns(Task.FromResult(existingEdges));

        // Act
        var result = await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        Assert.Equal(1, result.Count()); 
        Assert.Contains(result, edge => edge.Name == "NewHeader");
        Assert.DoesNotContain(result, edge => edge.Name == "Header1");
        Assert.DoesNotContain(result, edge => edge.Name == "Header2");
        await _attributeEdgeRepository.Received().AddRangeAsync(Arg.Any<IEnumerable<AttributeEdge>>());
    }
    
    
    
    [Fact]
    public async Task ProcessFromToAsync_HandlesEmptyHeadersCorrectly()
    {
        // Arrange
        var headers = new List<string>();
        var from = "HeaderFrom";
        var to = "HeaderTo";

        // Act
        var result = await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        Assert.Empty(result); 
        await _attributeEdgeRepository.DidNotReceive().AddRangeAsync(Arg.Any<IEnumerable<AttributeEdge>>());
    }
    
}
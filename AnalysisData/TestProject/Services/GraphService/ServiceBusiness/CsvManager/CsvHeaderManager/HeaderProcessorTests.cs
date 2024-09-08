using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager;
using NSubstitute;

namespace TestProject.Services.GraphService.ServiceBusiness.CsvManager.CsvHeaderManager;

public class HeaderProcessorTests
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly HeaderProcessor _headerProcessor;

    public HeaderProcessorTests()
    {
        _attributeNodeRepository = Substitute.For<IAttributeNodeRepository>();
        _headerProcessor = new HeaderProcessor(_attributeNodeRepository);
    }

    [Fact]
    public async Task ProcessHeadersAsync_ShouldAddNewHeaders_WhenHeadersAreUnique()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "Header3" };
        var uniqueAttribute = "UniqueHeader";
        
        _attributeNodeRepository.GetByNameAsync(Arg.Any<string>()).Returns((AttributeNode)null);

        // Act
        var result = await _headerProcessor.ProcessHeadersAsync(headers, uniqueAttribute);

        // Assert
        await _attributeNodeRepository.Received(3).GetByNameAsync(Arg.Any<string>()); 
        await _attributeNodeRepository.Received(1).AddRangeAsync(Arg.Any<IEnumerable<AttributeNode>>());
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task ProcessHeadersAsync_ShouldNotAddUniqueAttribute_WhenItIsInHeaders()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "UniqueHeader" };
        var uniqueAttribute = "UniqueHeader";
        
        _attributeNodeRepository.GetByNameAsync(Arg.Any<string>()).Returns((AttributeNode)null);

        // Act
        var result = await _headerProcessor.ProcessHeadersAsync(headers, uniqueAttribute);

        // Assert
        await _attributeNodeRepository.Received(2).GetByNameAsync(Arg.Any<string>()); 
        await _attributeNodeRepository.Received(1).AddRangeAsync(Arg.Any<IEnumerable<AttributeNode>>());
        Assert.Equal(2, result.Count()); 
    }
}
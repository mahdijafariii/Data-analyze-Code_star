using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class HeaderProcessorTests
{
    private readonly IAttributeNodeRepository _attributeNodeRepository;
    private readonly HeaderProcessor _sut;

    public HeaderProcessorTests()
    {
        _attributeNodeRepository = Substitute.For<IAttributeNodeRepository>();
        _sut = new HeaderProcessor(_attributeNodeRepository);
    }

    [Fact]
    public async Task ProcessHeadersAsync_ShouldAddNewAttributes_WhenHeadersDoNotExist()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "UniqueHeader" };
        var uniqueAttribute = "UniqueHeader";

        _attributeNodeRepository.GetByNameAsync(Arg.Any<string>()).Returns(Task.FromResult<AttributeNode>(null));

        // Act
        await _sut.ProcessHeadersAsync(headers, uniqueAttribute);

        // Assert
        await _attributeNodeRepository.Received().GetByNameAsync("Header1");
        await _attributeNodeRepository.Received().GetByNameAsync("Header2");
        await _attributeNodeRepository.DidNotReceive().GetByNameAsync(uniqueAttribute);
        await _attributeNodeRepository.Received().AddAsync(Arg.Is<AttributeNode>(a => a.Name == "Header1"));
        await _attributeNodeRepository.Received().AddAsync(Arg.Is<AttributeNode>(a => a.Name == "Header2"));
        await _attributeNodeRepository.Received(2).AddAsync(Arg.Any<AttributeNode>());
    }

    [Fact]
    public async Task ProcessHeadersAsync_ShouldNotAddAttributes_WhenHeadersExist()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "UniqueHeader" };
        var uniqueAttribute = "UniqueHeader";

        _attributeNodeRepository.GetByNameAsync(Arg.Any<string>()).Returns(Task.FromResult(new AttributeNode()));

        // Act
        await _sut.ProcessHeadersAsync(headers, uniqueAttribute);

        // Assert
        await _attributeNodeRepository.Received().GetByNameAsync("Header1");
        await _attributeNodeRepository.Received().GetByNameAsync("Header2");
        await _attributeNodeRepository.DidNotReceive().GetByNameAsync(uniqueAttribute);
        await _attributeNodeRepository.DidNotReceive().AddAsync(Arg.Any<AttributeNode>());
    }
}
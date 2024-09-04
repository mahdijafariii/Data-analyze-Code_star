using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

public class FromToProcessorTests
{
    private readonly IAttributeEdgeRepository _attributeEdgeRepository;
    private readonly FromToProcessor _fromToProcessor;

    public FromToProcessorTests()
    {
        _attributeEdgeRepository = Substitute.For<IAttributeEdgeRepository>();
        _fromToProcessor = new FromToProcessor(_attributeEdgeRepository);
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldIgnoreFromAndToHeaders()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2", "From", "To" };
        var from = "From";
        var to = "To";

        _attributeEdgeRepository
            .GetByNameAsync(Arg.Any<string>())
            .Returns(Task.FromResult<AttributeEdge>(null));

        // Act
        await _fromToProcessor.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository
            .Received(1)
            .AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Header1"));

        await _attributeEdgeRepository
            .Received(1)
            .AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Header2"));

        await _attributeEdgeRepository
            .DidNotReceive()
            .AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "From"));

        await _attributeEdgeRepository
            .DidNotReceive()
            .AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "To"));
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldAddNewEdgesIfNotExists()
    {
        // Arrange
        var headers = new List<string> { "Header1", "Header2" };
        var from = "From";
        var to = "To";

        _attributeEdgeRepository
            .GetByNameAsync(Arg.Any<string>())
            .Returns(Task.FromResult<AttributeEdge>(null)); 

        // Act
        await _fromToProcessor.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository
            .Received(1)
            .AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Header1"));

        await _attributeEdgeRepository
            .Received(1)
            .AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Header2"));
    }
}
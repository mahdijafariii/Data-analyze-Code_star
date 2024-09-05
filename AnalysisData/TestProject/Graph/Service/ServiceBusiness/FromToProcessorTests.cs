using AnalysisData.Graph.Model.Edge;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Service.ServiceBusiness;
using NSubstitute;

namespace TestProject.Graph.Service.ServiceBusiness;

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
    public async Task ProcessFromToAsync_ShouldExcludeFromAndToHeaders()
    {
        // Arrange
        var headers = new List<string> { "From", "To", "Attribute1", "Attribute2" };
        var from = "From";
        var to = "To";

        // Act
        await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository.Received(1).AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Attribute1"));
        await _attributeEdgeRepository.Received(1).AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Attribute2"));
        await _attributeEdgeRepository.DidNotReceive().AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "From" || ae.Name == "To"));
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldAddAttributeEdge_WhenNotExists()
    {
        // Arrange
        var headers = new List<string> { "Attribute1" };
        var from = "From";
        var to = "To";

        _attributeEdgeRepository.GetByNameAsync(Arg.Any<string>()).Returns((AttributeEdge)null);

        // Act
        await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository.Received(1).AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Attribute1"));
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldNotAddAttributeEdge_WhenAlreadyExists()
    {
        // Arrange
        var headers = new List<string> { "Attribute1" };
        var from = "From";
        var to = "To";

        var existingAttributeEdge = new AttributeEdge { Name = "Attribute1" };
        _attributeEdgeRepository.GetByNameAsync("Attribute1").Returns(existingAttributeEdge); 

        // Act
        await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository.DidNotReceive().AddAsync(Arg.Any<AttributeEdge>());
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldProcessMultipleAttributes()
    {
        // Arrange
        var headers = new List<string> { "Attribute1", "Attribute2", "From", "To" };
        var from = "From";
        var to = "To";

        _attributeEdgeRepository.GetByNameAsync(Arg.Any<string>()).Returns((AttributeEdge)null); 

        // Act
        await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository.Received(1).AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Attribute1"));
        await _attributeEdgeRepository.Received(1).AddAsync(Arg.Is<AttributeEdge>(ae => ae.Name == "Attribute2"));
    }

    [Fact]
    public async Task ProcessFromToAsync_ShouldNotAddAnything_WhenNoHeaders()
    {
        // Arrange
        var headers = new List<string>();
        var from = "From";
        var to = "To";

        // Act
        await _sut.ProcessFromToAsync(headers, from, to);

        // Assert
        await _attributeEdgeRepository.DidNotReceive().AddAsync(Arg.Any<AttributeEdge>());
    }
}
using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class ValueNodeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ValueNodeRepository _sut;

    public ValueNodeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new ValueNodeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddValueNodeToDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "TestAttribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        var valueNode = new ValueNode
        {
            Id = Guid.NewGuid(),
            AttributeId = attributeNode.Id,
            Value = "TestValue"
        };

        // Act
        await _sut.AddAsync(valueNode);

        // Assert
        var result = await context.ValueNodes.FindAsync(valueNode.Id);
        Assert.NotNull(result);
        Assert.Equal("TestValue", result.Value);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddValidValueNodes()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode1 = new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute 1" };
        var attributeNode2 = new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute 2" };
        await context.AttributeNodes.AddRangeAsync(attributeNode1, attributeNode2);
        await context.SaveChangesAsync();

        var valueNodes = new List<ValueNode>
        {
            new() { Id = Guid.NewGuid(), AttributeId = attributeNode1.Id, Value = "Value 1" },
            new() { Id = Guid.NewGuid(), AttributeId = attributeNode2.Id, Value = "Value 2" },
            new() { Id = Guid.NewGuid(), AttributeId = Guid.NewGuid(), Value = "Invalid Value" }
        };

        // Act
        await _sut.AddRangeAsync(valueNodes);

        // Assert
        var allValueNodes = await context.ValueNodes.ToListAsync();
        Assert.Equal(2, allValueNodes.Count);
        Assert.DoesNotContain(allValueNodes, vn => vn.Value == "Invalid Value");
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllValueNodes()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "Test Attribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        var valueNode1 = new ValueNode { Id = Guid.NewGuid(), AttributeId = attributeNode.Id, Value = "Value 1" };
        var valueNode2 = new ValueNode { Id = Guid.NewGuid(), AttributeId = attributeNode.Id, Value = "Value 2" };
        await context.ValueNodes.AddRangeAsync(valueNode1, valueNode2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2,result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnValueNodeWithGivenId()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "Test Attribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        var valueNode = new ValueNode { Id = Guid.NewGuid(), AttributeId = attributeNode.Id, Value = "Test Value" };
        await context.ValueNodes.AddAsync(valueNode);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(valueNode.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(valueNode.Id, result.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveValueNodeFromDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "Test Attribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        var valueNode = new ValueNode { Id = Guid.NewGuid(), AttributeId = attributeNode.Id, Value = "Test Value" };
        await context.ValueNodes.AddAsync(valueNode);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(valueNode.Id);

        // Assert
        var result = await context.ValueNodes.FindAsync(valueNode.Id);
        Assert.Null(result);
    }
}
using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class AttributeNodeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly AttributeNodeRepository _sut;

    public AttributeNodeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new AttributeNodeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddAttributeNodeToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode
        {
            Id = Guid.NewGuid(),
            Name = "TestAttribute"
        };

        // Act
        await _sut.AddAsync(attributeNode);

        // Assert
        var result = await context.AttributeNodes.FindAsync(attributeNode.Id);
        Assert.NotNull(result);
        Assert.Equal("TestAttribute", result.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAttributeNodes_WhenAttributeNodesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode1 = new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute1" };
        var attributeNode2 = new AttributeNode { Id = Guid.NewGuid(), Name = "Attribute2" };

        await context.AttributeNodes.AddRangeAsync(attributeNode1, attributeNode2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAttributeNodeWithGivenId_WhenAttributeNodesWithInputIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "TestAttribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(attributeNode.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(attributeNode.Id, result.Id);
        Assert.Equal("TestAttribute", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnAttributeNodeWithGivenName_WhenAttributeNodesWithInputNameExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "UniqueAttribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByNameAsync("UniqueAttribute");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UniqueAttribute", result.Name);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddNewAttributeNodes_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var existingNodeId = Guid.NewGuid();
        var existingNode = new AttributeNode { Id = existingNodeId, Name = "ExistingAttribute" };
        await context.AttributeNodes.AddAsync(existingNode);
        await context.SaveChangesAsync();

        var newAttributeNodes = new List<AttributeNode>
        {
            new() { Id = Guid.NewGuid(), Name = "NewAttribute1" },
            new() { Id = Guid.NewGuid(), Name = "NewAttribute2" }
        };

        // Act
        await _sut.AddRangeAsync(newAttributeNodes);

        // Assert
        var allNodes = await context.AttributeNodes.ToListAsync();
        Assert.Equal(3, allNodes.Count);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAttributeNodeFromDatabase_WhenAttributeNodeWithInputIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeNode = new AttributeNode { Id = Guid.NewGuid(), Name = "TestAttribute" };
        await context.AttributeNodes.AddAsync(attributeNode);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(attributeNode.Id);

        // Assert
        var result = await context.AttributeNodes.FindAsync(attributeNode.Id);
        Assert.Null(result);
    }
}
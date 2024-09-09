using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class EntityNodeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly EntityNodeRepository _sut;

    public EntityNodeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new EntityNodeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityNodeToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityNode = new EntityNode
        {
            Id = Guid.NewGuid(),
            Name = "TestNode",
            NodeFileReferenceId = 1
        };

        // Act
        await _sut.AddAsync(entityNode);
        var result = await context.EntityNodes.FindAsync(entityNode.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestNode", result.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntityNodes()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityNode1 = new EntityNode { Id = Guid.NewGuid(), Name = "Node 1", NodeFileReferenceId = 1 };
        var entityNode2 = new EntityNode { Id = Guid.NewGuid(), Name = "Node 2", NodeFileReferenceId = 2 };

        await context.EntityNodes.AddRangeAsync(entityNode1, entityNode2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2,result.Count());
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnEntityNodeWithGivenName()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityNode = new EntityNode { Id = Guid.NewGuid(), Name = "UniqueNode", NodeFileReferenceId = 1 };
        await context.EntityNodes.AddAsync(entityNode);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByNameAsync("UniqueNode");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UniqueNode", result.Name);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddNewEntityNodes()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var existingNodeId = Guid.NewGuid();
        var existingNode = new EntityNode { Id = existingNodeId, Name = "ExistingNode", NodeFileReferenceId = 1 };
        await context.EntityNodes.AddAsync(existingNode);
        await context.SaveChangesAsync();

        var newEntityNodes = new List<EntityNode>
        {
            new() { Id = Guid.NewGuid(), Name = "NewNode1", NodeFileReferenceId = 2 },
            new() { Id = Guid.NewGuid(), Name = "NewNode2", NodeFileReferenceId = 3 }
        };

        // Act
        await _sut.AddRangeAsync(newEntityNodes);
        var allNodes = await context.EntityNodes.ToListAsync();

        // Assert
        Assert.Equal(3,allNodes.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntityNodeWithGivenId()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityNode = new EntityNode { Id = Guid.NewGuid(), Name = "TestNode", NodeFileReferenceId = 1 };
        await context.EntityNodes.AddAsync(entityNode);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(entityNode.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityNode.Id, result.Id);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntityNodeFromDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityNode = new EntityNode { Id = Guid.NewGuid(), Name = "TestNode", NodeFileReferenceId = 1 };
        await context.EntityNodes.AddAsync(entityNode);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(entityNode.Id);

        // Assert
        var result = await context.EntityNodes.FindAsync(entityNode.Id);
        Assert.Null(result);
    }
}

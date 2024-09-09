using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public class EntityEdgeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly EntityEdgeRepository _sut;

    public EntityEdgeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new EntityEdgeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityEdgeToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityEdge = new EntityEdge
        {
            Id = Guid.NewGuid(),
            EntityIDSource = Guid.NewGuid(),
            EntityIDTarget = Guid.NewGuid(),
        };

        // Act
        await _sut.AddAsync(entityEdge);
        var result = await context.EntityEdges.FindAsync(entityEdge.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entityEdge.Id, result.Id);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddOnlyValidEntityEdges_WhenEntityEdgeIsValid()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var validEntityId = Guid.NewGuid();
        var invalidEntityId = Guid.NewGuid();

        context.EntityNodes.Add(new EntityNode { Id = validEntityId, Name = "edge"});
        await context.SaveChangesAsync();

        var edgeId1 = Guid.NewGuid();
        var edgeId2 = Guid.NewGuid();

        var entityEdges = new List<EntityEdge>
        {
            new() { Id = edgeId1, EntityIDSource = validEntityId, EntityIDTarget = Guid.NewGuid() },
            new() { Id = edgeId2, EntityIDSource = invalidEntityId, EntityIDTarget = Guid.NewGuid() }
        };

        // Act
        await _sut.AddRangeAsync(entityEdges);
        var result = await context.EntityEdges.ToListAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(edgeId1, result[0].Id);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntityEdges_WhenEntityEdgesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edges = new List<EntityEdge>
        {
            new() { Id = Guid.NewGuid(), EntityIDSource = Guid.NewGuid(), EntityIDTarget = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), EntityIDSource = Guid.NewGuid(), EntityIDTarget = Guid.NewGuid() }
        };
        await context.EntityEdges.AddRangeAsync(edges);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntityEdge_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edgeId = Guid.NewGuid();
        var entityEdge = new EntityEdge
        {
            Id = edgeId,
            EntityIDSource = Guid.NewGuid(),
            EntityIDTarget = Guid.NewGuid(),
        };
        await context.EntityEdges.AddAsync(entityEdge);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(edgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(edgeId, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edgeId = Guid.NewGuid();
        var entityEdge = new EntityEdge
        {
            Id = edgeId,
            EntityIDSource = Guid.NewGuid(),
            EntityIDTarget = Guid.NewGuid(),
        };
        await context.EntityEdges.AddAsync(entityEdge);
        await context.SaveChangesAsync();
        
        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task
        FindNodeLoopsAsync_ShouldReturnEntityEdgesWithMatchingEntityIDSourceOrEntityIDTarget_WhenEntityEdgesWithMatchingEntityIDSourceOrEntityIDTargetExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityId = Guid.NewGuid();
        var edgeId1 = Guid.NewGuid();
        var edgeId2 = Guid.NewGuid();

        var edges = new List<EntityEdge>
        {
            new() { Id = edgeId1, EntityIDSource = entityId, EntityIDTarget = Guid.NewGuid() },
            new() { Id = edgeId2, EntityIDSource = Guid.NewGuid(), EntityIDTarget = entityId }
        };
        await context.EntityEdges.AddRangeAsync(edges);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.FindNodeLoopsAsync(entityId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Id == edgeId1);
        Assert.Contains(result, x => x.Id == edgeId2);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntityEdge_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edgeId = Guid.NewGuid();
        var entityEdge = new EntityEdge
        {
            Id = edgeId,
            EntityIDSource = Guid.NewGuid(),
            EntityIDTarget = Guid.NewGuid(),
        };
        await context.EntityEdges.AddAsync(entityEdge);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(edgeId);
        var result = await context.EntityEdges.FindAsync(edgeId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDoNothing_WhenIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edgeId = Guid.NewGuid();
        var entityEdge = new EntityEdge
        {
            Id = edgeId,
            EntityIDSource = Guid.NewGuid(),
            EntityIDTarget = Guid.NewGuid(),
        };
        await context.EntityEdges.AddAsync(entityEdge);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(Guid.NewGuid());

        // Assert
        var result = await context.EntityEdges.CountAsync();
        Assert.Equal(1, result);
    }
}
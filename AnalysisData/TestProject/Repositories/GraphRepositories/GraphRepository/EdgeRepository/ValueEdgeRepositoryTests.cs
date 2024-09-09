using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class ValueEdgeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly ValueEdgeRepository _sut;

    public ValueEdgeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new ValueEdgeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddValueEdgeToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var valueEdge = new ValueEdge
        {
            Id = Guid.NewGuid(),
            EntityId = Guid.NewGuid(),
            Value = "100"
        };

        // Act
        await _sut.AddAsync(valueEdge);
        var result = await context.ValueEdges.FindAsync(valueEdge.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(valueEdge.Id, result.Id);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddOnlyValidValueEdges_WhenValueEdgeIsValid()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var validEntityId = Guid.NewGuid();
        var invalidEntityId = Guid.NewGuid();

        // Add existing EntityEdges
        context.EntityEdges.Add(new EntityEdge { Id = validEntityId });
        await context.SaveChangesAsync();

        var valueEdges = new List<ValueEdge>
        {
            new() { Id = Guid.NewGuid(), EntityId = validEntityId, Value = "200" },
            new() { Id = Guid.NewGuid(), EntityId = invalidEntityId, Value = "300" }
        };

        // Act
        await _sut.AddRangeAsync(valueEdges);
        var result = await context.ValueEdges.ToListAsync();

        // Assert
        Assert.Single(result); 
        Assert.Equal("200", result[0].Value);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllValueEdges_WhenValueEdgesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var valueEdges = new List<ValueEdge>
        {
            new() { Id = Guid.NewGuid(), EntityId = Guid.NewGuid(), Value = "100" },
            new() { Id = Guid.NewGuid(), EntityId = Guid.NewGuid(), Value = "200" }
        };
        await context.ValueEdges.AddRangeAsync(valueEdges);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnValueEdge_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var valueEdgeId = Guid.NewGuid();
        var valueEdge = new ValueEdge
        {
            Id = valueEdgeId,
            EntityId = Guid.NewGuid(),
            Value = "100"
        };
        await context.ValueEdges.AddAsync(valueEdge);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(valueEdgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(valueEdgeId, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var valueEdgeId = Guid.NewGuid();
        var valueEdge = new ValueEdge
        {
            Id = valueEdgeId,
            EntityId = Guid.NewGuid(),
            Value = "100"
        };
        await context.ValueEdges.AddAsync(valueEdge);
        await context.SaveChangesAsync();
        
        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveValueEdge_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var valueEdgeId = Guid.NewGuid();
        var valueEdge = new ValueEdge
        {
            Id = valueEdgeId,
            EntityId = Guid.NewGuid(),
            Value = "100"
        };
        await context.ValueEdges.AddAsync(valueEdge);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(valueEdgeId);
        var result = await context.ValueEdges.FindAsync(valueEdgeId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDoNothing_WhenIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var valueEdgeId = Guid.NewGuid();
        var valueEdge = new ValueEdge
        {
            Id = valueEdgeId,
            EntityId = Guid.NewGuid(),
            Value = "100"
        };
        await context.ValueEdges.AddAsync(valueEdge);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(Guid.NewGuid());

        // Assert
        var result = await context.ValueEdges.CountAsync();
        Assert.Equal(1, result);
    }
}

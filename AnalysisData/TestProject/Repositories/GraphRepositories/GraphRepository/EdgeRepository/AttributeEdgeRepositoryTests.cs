using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public class AttributeEdgeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly AttributeEdgeRepository _sut;

    public AttributeEdgeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new AttributeEdgeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddAttributeEdgeToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var attributeEdge = new AttributeEdge
        {
            Id = Guid.NewGuid(),
            Name = "Edge1"
        };

        // Act
        await _sut.AddAsync(attributeEdge);
        var result = await context.AttributeEdges.FindAsync(attributeEdge.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(attributeEdge.Id, result.Id);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddOnlyNonExistingAttributeEdges_WhenAttributeEdgeExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var existingEdge = new AttributeEdge
        {
            Id = Guid.NewGuid(),
            Name = "Edge1"
        };
        await context.AttributeEdges.AddAsync(existingEdge);
        await context.SaveChangesAsync();

        var newEdges = new List<AttributeEdge>
        {
            new() { Id = existingEdge.Id, Name = "Edge1" }, 
            new() { Id = Guid.NewGuid(), Name = "Edge2" }   
        };

        // Act
        await _sut.AddRangeAsync(newEdges);
        var result = await context.AttributeEdges.ToListAsync();

        // Assert
        Assert.Equal(2, result.Count); 
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAttributeEdges_WhenAttributeEdgesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edges = new List<AttributeEdge>
        {
            new() { Id = Guid.NewGuid(), Name = "Edge1" },
            new() { Id = Guid.NewGuid(), Name = "Edge2" }
        };
        await context.AttributeEdges.AddRangeAsync(edges);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAttributeEdge_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edgeId = Guid.NewGuid();
        var attributeEdge = new AttributeEdge { Id = edgeId, Name = "Edge1" };
        await context.AttributeEdges.AddAsync(attributeEdge);
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
        var attributeEdge = new AttributeEdge { Id = edgeId, Name = "Edge1" };
        await context.AttributeEdges.AddAsync(attributeEdge);
        await context.SaveChangesAsync();
        
        // Act
        var result = await _sut.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByNamesAsync_ShouldReturnAttributeEdgesWithMatchingNames_WhenAttributeEdgesWithMatchingNamesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edges = new List<AttributeEdge>
        {
            new() { Id = Guid.NewGuid(), Name = "Edge1" },
            new() { Id = Guid.NewGuid(), Name = "Edge2" }
        };
        await context.AttributeEdges.AddRangeAsync(edges);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByNamesAsync(new[] { "Edge1", "Edge2" });

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, x => x.Name == "Edge1");
        Assert.Contains(result, x => x.Name == "Edge2");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveAttributeEdge_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var edgeId = Guid.NewGuid();
        var attributeEdge = new AttributeEdge { Id = edgeId, Name = "Edge1" };
        await context.AttributeEdges.AddAsync(attributeEdge);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(edgeId);
        var result = await context.AttributeEdges.FindAsync(edgeId);

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
        var attributeEdge = new AttributeEdge { Id = edgeId, Name = "Edge1" };
        await context.AttributeEdges.AddAsync(attributeEdge);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(Guid.NewGuid());

        // Assert
        var result = await context.AttributeEdges.CountAsync();
        Assert.Equal(1, result); 
    }
}

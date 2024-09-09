using AnalysisData.Data;
using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public class GraphEdgeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly GraphEdgeRepository _sut;

    public GraphEdgeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new GraphEdgeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task GetEdgeAttributeValues_ShouldReturnEdgeInformationDtos_WhenEdgeAttributeValuesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var entityEdgeId = Guid.NewGuid();
        var entityEdge = new EntityEdge
        {
            Id = entityEdgeId,
            EntityIDSource = Guid.NewGuid(),
            EntityIDTarget = Guid.NewGuid(),
        };
        var attribute = new AttributeEdge
        {
            Id = Guid.NewGuid(),
            Name = "Attribute1"
        };

        var valueEdge = new ValueEdge
        {
            Id = Guid.NewGuid(),
            Entity = entityEdge,
            Attribute = attribute,
            Value = "123"
        };

        await context.EntityEdges.AddAsync(entityEdge);
        await context.AttributeEdges.AddAsync(attribute);
        await context.ValueEdges.AddAsync(valueEdge);
        await context.SaveChangesAsync();

        var edgeInformationDto = new EdgeInformationDto()
        {
            Attribute = "Attribute1",
            Value = "123"
        };
        
        
        // Act
        var result = await _sut.GetEdgeAttributeValues(entityEdgeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(edgeInformationDto.Value,result.First().Value);
    }

    [Fact]
    public async Task IsEdgeAccessibleByUser_ShouldReturnTrue_WhenUserHasAccessToEdge()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntityId = 1;
        var entityId = Guid.NewGuid();

        var entityEdge = new EntityEdge { Id = Guid.NewGuid(), EntityIDSource = entityId };
        var entityNode = new EntityNode { Id = entityId, NodeFileReferenceId = fileEntityId,Name = "node"};
        var fileEntity = new FileEntity { Id = fileEntityId,CategoryId = 1,FileName = "test",UploadDate = DateTime.Now};
        var userFile = new UserFile
        {
            UserId = userId,
            FileEntity = fileEntity
        };

        await context.EntityEdges.AddAsync(entityEdge);
        await context.EntityNodes.AddAsync(entityNode);
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.IsEdgeAccessibleByUser(userId.ToString(), entityEdge.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsEdgeAccessibleByUser_ShouldReturnFalse_WhenUserDoesNotHaveAccessToEdge()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var fileEntityId = 1;
        var entityId = Guid.NewGuid();

        var entityEdge = new EntityEdge { Id = Guid.NewGuid(), EntityIDSource = entityId };
        var entityNode = new EntityNode { Id = entityId, NodeFileReferenceId = fileEntityId,Name = "node"};
        var fileEntity = new FileEntity { Id = fileEntityId,CategoryId = 1,FileName = "test",UploadDate = DateTime.Now};

        var userFile = new UserFile
        {
            UserId = anotherUserId,
            FileEntity = fileEntity
        };

        await context.EntityEdges.AddAsync(entityEdge);
        await context.EntityNodes.AddAsync(entityNode);
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.IsEdgeAccessibleByUser(userId.ToString(), entityEdge.Id);

        // Assert
        Assert.False(result);
    }
}
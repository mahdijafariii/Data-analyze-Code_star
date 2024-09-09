using AnalysisData.Data;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class GraphNodeRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly GraphNodeRepository _sut;

    public GraphNodeRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new GraphNodeRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task GetEntityNodesForAdminAsync_ShouldReturnAllEntityNodes_WhenNodesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "Node1" };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "Node2" };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetEntityNodesForAdminAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetEntityNodesForAdminWithCategoryIdAsync_ShouldReturnNodesMatchingCategoryId_WhenWithCategoryIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var fileEntity = new FileEntity { Id = 1, CategoryId = 100, FileName = "test" };
        var node1 = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileEntity.Id, Name = "node1" };
        var node2 = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = 2, Name = "node2" };

        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetEntityNodesForAdminWithCategoryIdAsync(100);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(node1.Id, result.First().Id);
    }

    [Fact]
    public async Task GetEntityNodesForUserAsync_ShouldReturnNodesForUserFiles_WhenNodesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntity = new FileEntity { Id = 1, FileName = "test" };
        var node = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileEntity.Id, Name = "node" };
        var userFile = new UserFile { UserId = userId, FileEntity = fileEntity };

        await context.EntityNodes.AddAsync(node);
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetEntityNodesForUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(node.Id, result.First().Id);
    }

    [Fact]
    public async Task IsNodeAccessibleByUser_ShouldReturnTrue_WhenUserHasAccess()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntity = new FileEntity { Id = 1, FileName = "test" };
        var node = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileEntity.Id, Name = "node" };
        var userFile = new UserFile { UserId = userId, FileEntity = fileEntity };

        await context.EntityNodes.AddAsync(node);
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.IsNodeAccessibleByUser(userId, node.Id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsNodeAccessibleByUser_ShouldReturnFalse_WhenUserDoesNotHaveAccess()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntity = new FileEntity { Id = 1, FileName = "test" };
        var node = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileEntity.Id, Name = "node" };

        await context.EntityNodes.AddAsync(node);
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.IsNodeAccessibleByUser(userId, node.Id);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetNodeAttributeValueAsync_ShouldReturnNodeInformationDtos_WhenNodeAttributeValueExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var nodeId = Guid.NewGuid();
        var entity = new EntityNode()
        {
            Id = nodeId,
            Name = "node"
        };
        var attribute = new AttributeNode()
        {
            Id = Guid.NewGuid(),
            Name = "Attribute1"
        };
        var valueNode = new ValueNode
        {
            Id = Guid.NewGuid(),
            Entity = entity,
            Attribute = attribute,
            Value = "123"
        };

        await context.EntityNodes.AddAsync(entity);
        await context.AttributeNodes.AddAsync(attribute);
        await context.ValueNodes.AddAsync(valueNode);
        await context.SaveChangesAsync();

        var nodeInformationDto = new NodeInformationDto()
        {
            Attribute = "Attribute1",
            Value = "123"
        };

        // Act
        var result = await _sut.GetNodeAttributeValueAsync(nodeId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(nodeInformationDto.Attribute, result.First().Attribute);
    }

    [Fact]
    public async Task
        GetNodeContainSearchInputForAdminAsync_ShouldReturnNodesContainingInput_WhenNodesContainingInputExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "NodeSearch" };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "AnotherNode" };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNodeContainSearchInputForAdminAsync("Search");

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(node1.Id, result.First().Id);
    }

    [Fact]
    public async Task GetNodeStartsWithSearchInputForAdminAsync_ShouldReturnNodesStartingWithInput_WhenNodeStartsWithSearchInputExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "StartNode" };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "NodeEnd" };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNodeStartsWithSearchInputForAdminAsync("Start");

        // Assert
        Assert.Single(result);
        Assert.Equal(node1.Id, result.First().Id);
    }

    [Fact]
    public async Task GetNodeEndsWithSearchInputForAdminAsync_ShouldReturnNodesEndingWithInput_WhenNodeEndsWithSearchInputExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "NodeEnd" };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "StartNode" };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNodeEndsWithSearchInputForAdminAsync("End");

        // Assert
        Assert.Single(result);
        Assert.Equal(node1.Id, result.First().Id);
    }

    [Fact]
    public async Task GetNodeContainSearchInputForUserAsync_ShouldReturnNodesContainingInputForUser_WhenNodeContainSearchInputExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntity = new FileEntity { Id = 1, FileName = "test" };
        var node1 = new EntityNode
        {
            Id = Guid.NewGuid(),
            Name = "UserNodeSearch",
            NodeFileReferenceId = fileEntity.Id
        };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "AnotherNode" };
        var userFile = new UserFile { UserId = userId, FileEntity = fileEntity };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNodeContainSearchInputForUserAsync(userId, "UserNodeSearch");

        // Assert
        Assert.Single(result);
        Assert.Equal(node1.Id, result.First().Id);
    }

    [Fact]
    public async Task GetNodeStartsWithSearchInputForUserAsync_ShouldReturnNodesStartingWithInputForUser_WhenNodeStartsWithSearchInputExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntity = new FileEntity { Id = 1, FileName = "test" };
        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "UserStartNode", NodeFileReferenceId = fileEntity.Id };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "NodeEnd" };
        var userFile = new UserFile { UserId = userId, FileEntity = fileEntity };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNodeStartsWithSearchInputForUserAsync(userId, "UserStart");

        // Assert
        Assert.Single(result);
        Assert.Equal(node1.Id, result.First().Id);
    }

    [Fact]
    public async Task GetNodeEndsWithSearchInputForUserAsync_ShouldReturnNodesEndingWithInputForUser_WhenNodeEndsWithSearchInputExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntity = new FileEntity { Id = 1, FileName = "test" };
        var node1 = new EntityNode { Id = Guid.NewGuid(), Name = "UserNodeEnd", NodeFileReferenceId = fileEntity.Id };
        var node2 = new EntityNode { Id = Guid.NewGuid(), Name = "StartNode" };
        var userFile = new UserFile { UserId = userId, FileEntity = fileEntity };

        await context.EntityNodes.AddRangeAsync(node1, node2);
        await context.UserFiles.AddAsync(userFile);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNodeEndsWithSearchInputForUserAsync(userId, "NodeEnd");

        // Assert
        Assert.Single(result);
        Assert.Equal(node1.Id, result.First().Id);
    }
    
    [Fact]
    public async Task GetEntityNodeForUserWithCategoryIdAsync_ShouldReturnNodesMatchingUserFilesAndCategory_WhenEntityNodeWithInputCategoryIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileId1 = 1;
        var fileId2 = 2;
        var fileId3 = 3;
        var categoryId1 = 100;
        var categoryId2 = 200;

        var fileEntity1 = new FileEntity { Id = fileId1, CategoryId = categoryId2,FileName = "test1"};
        var fileEntity2 = new FileEntity { Id = fileId2, CategoryId = categoryId1, FileName="test2" };
        var fileEntity3 = new FileEntity { Id = fileId3, CategoryId = categoryId1, FileName="test3" };

        var node1 = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileId1 ,Name = "node1"};
        var node2 = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileId2,Name = "node2"};
        var node3 = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileId2,Name = "node3"};
        var node4 = new EntityNode { Id = Guid.NewGuid(), NodeFileReferenceId = fileId3,Name = "node4"}; 

        var userFile1 = new UserFile { UserId = userId, FileId = fileId1 };
        var userFile2 = new UserFile { UserId = userId, FileId = fileId2 };
        var userFile3 = new UserFile { UserId = userId, FileId = fileId3 };
        
        await context.FileUploadedDb.AddRangeAsync(fileEntity1, fileEntity2,fileEntity3);
        await context.EntityNodes.AddRangeAsync(node1, node2, node3, node4);
        await context.UserFiles.AddRangeAsync(userFile1, userFile2,userFile3);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetEntityNodeForUserWithCategoryIdAsync(userId, categoryId1);

        // Assert
        Assert.Equal(3, result.Count()); 
        Assert.Contains(result, n => n.Id == node4.Id);
        Assert.Contains(result, n => n.Id == node2.Id);
        Assert.Contains(result, n => n.Id == node3.Id);
        Assert.DoesNotContain(result, n => n.Id == node1.Id);
    }
    
}
using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


public class FileUploadedRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly FileUploadedRepository _sut;

    public FileUploadedRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new FileUploadedRepository(CreateDbContext()); 
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task AddAsync_ShouldAddFileEntityToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var fileEntity = new FileEntity
        {
            FileName = "UserFile",
            UploaderId = Guid.NewGuid(),
            CategoryId = 1
        };

        // Act
        await _sut.AddAsync(fileEntity);
        var result = await context.FileUploadedDb.FirstOrDefaultAsync(x => x.FileName == "UserFile");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UserFile", result.FileName);
    }

    [Fact]
    public async Task GetUploadedFilesAsync_ShouldReturnCorrectPagedFiles_WhenFilesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var categories = new List<Category>()
        {
            new() { Id = 1, Name = "category1" },
            new() { Id = 2, Name = "category2" }
        };
        await context.Categories.AddRangeAsync(categories);

        var fileEntities = new List<FileEntity>
        {
            new() { FileName = "File1", UploaderId = Guid.NewGuid(), CategoryId = 1 },
            new() { FileName = "File2", UploaderId = Guid.NewGuid(), CategoryId = 1 },
            new() { FileName = "File3", UploaderId = Guid.NewGuid(), CategoryId = 1 }
        };
        await context.FileUploadedDb.AddRangeAsync(fileEntities);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetUploadedFilesAsync(page: 0, limit: 2);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFiles_WhenFilesExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var fileEntities = new List<FileEntity>
        {
            new() { FileName = "File1", UploaderId = Guid.NewGuid(), CategoryId = 1 },
            new() { FileName = "File2", UploaderId = Guid.NewGuid(), CategoryId = 1 }
        };
        await context.FileUploadedDb.AddRangeAsync(fileEntities);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFileEntity_WhenIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var fileEntity = new FileEntity { FileName = "File1", UploaderId = Guid.NewGuid(), CategoryId = 1 };
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(fileEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fileEntity.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var fileEntity = new FileEntity { FileName = "File1", UploaderId = Guid.NewGuid(), CategoryId = 1 };
        await context.FileUploadedDb.AddAsync(fileEntity);
        await context.SaveChangesAsync();
        
        // Act
        var result = await _sut.GetByIdAsync(999); 

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnFiles_WhenUserIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntities = new List<FileEntity>
        {
            new() { FileName = "File1", UploaderId = userId, CategoryId = 1 },
            new() { FileName = "File2", UploaderId = userId, CategoryId = 2 }
        };
        await context.FileUploadedDb.AddRangeAsync(fileEntities);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByUserIdAsync(userId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByUserIdAsync_ShouldReturnEmpty_WhenUserIdDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var userId = Guid.NewGuid();
        var fileEntities = new List<FileEntity>
        {
            new() { FileName = "File1", UploaderId = userId, CategoryId = 1 },
            new() { FileName = "File2", UploaderId = userId, CategoryId = 2 }
        };
        await context.FileUploadedDb.AddRangeAsync(fileEntities);
        await context.SaveChangesAsync();
        
        // Act
        var result = await _sut.GetByUserIdAsync(Guid.NewGuid()); 

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetNumberOfFileWithCategoryIdAsync_ShouldReturnCorrectCount_WhenCategoryIdExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var categoryId = 1;
        var fileEntities = new List<FileEntity>
        {
            new() { FileName = "File1", UploaderId = Guid.NewGuid(), CategoryId = categoryId },
            new() { FileName = "File2", UploaderId = Guid.NewGuid(), CategoryId = categoryId }
        };
        await context.FileUploadedDb.AddRangeAsync(fileEntities);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetNumberOfFileWithCategoryIdAsync(categoryId);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task GetTotalFilesCountAsync_ShouldReturnCorrectTotalCount_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var fileEntities = new List<FileEntity>
        {
            new() { FileName = "File1", UploaderId = Guid.NewGuid(), CategoryId = 1 },
            new() { FileName = "File2", UploaderId = Guid.NewGuid(), CategoryId = 2 },
            new() { FileName = "File3", UploaderId = Guid.NewGuid(), CategoryId = 3 }

        };
        await context.FileUploadedDb.AddRangeAsync(fileEntities);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetTotalFilesCountAsync();

        // Assert
        Assert.Equal(3, result);
    }
}
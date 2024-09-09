using AnalysisData.Data;
using AnalysisData.Models.GraphModel.Category;
using AnalysisData.Repositories.GraphRepositories.CategoryRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CategoryRepositoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly CategoryRepository _sut;

    public CategoryRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new CategoryRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCategories_WhenCategoryExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var category1 = new Category { Id = 1, Name = "Category1" };
        var category2 = new Category { Id = 2, Name = "Category2" };
        await context.Categories.AddRangeAsync(category1, category2);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        Assert.Equal(2,result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCategoryWithGivenId_WhenCategoryWithInputIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var category = new Category { Id = 1, Name = "Category1" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Category1", result.Name);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnCategoryWithGivenName_WhenCategoryWithInputNameExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var category = new Category { Id = 1, Name = "UniqueCategory" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetByNameAsync("UniqueCategory");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UniqueCategory", result.Name);
    }

    [Fact]
    public async Task AddAsync_ShouldAddCategoryToDatabase_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var category = new Category { Id = 1, Name = "NewCategory" };

        // Act
        await _sut.AddAsync(category);
        var result = await context.Categories.FindAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("NewCategory", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory_WhenCategoryWithInputIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var category = new Category { Id = 1, Name = "OldName" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        category.Name = "UpdatedName";
        await _sut.UpdateAsync(category);
        var result = await context.Categories.FindAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("UpdatedName", result.Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveCategoryFromDatabase_WhenCategoryWithInputIdExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var category = new Category { Id = 1, Name = "category" };
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        await _sut.DeleteAsync(1);
        var result = await context.Categories.FindAsync(1);

        // Assert
        Assert.Null(result);
    }
}

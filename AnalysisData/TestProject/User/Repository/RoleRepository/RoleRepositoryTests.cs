using System.Linq;
using System.Threading.Tasks;
using AnalysisData.Data;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.RoleRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;


public class RoleRepositoryTests
{
    private readonly RoleRepository _sut;
    private readonly ServiceProvider _serviceProvider;

    public RoleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(_ => new ApplicationDbContext(options));
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _sut = new RoleRepository(CreateDbContext());
    }

    private ApplicationDbContext CreateDbContext()
    {
        return _serviceProvider.GetRequiredService<ApplicationDbContext>();
    }

    [Fact]
    public async Task GetRoleByIdAsync_ShouldReturnRoleWithInputId_WhenRoleExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public async Task GetRoleByIdAsync_ShouldReturnNull_WhenRoleDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByIdAsync(2);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetRoleByNameAsync_ShouldReturnRoleWithInputRoleName_WhenRoleExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByNameAsync("Admin");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
    }

    [Fact]
    public async Task GetRoleByNameAsync_ShouldReturnNull_WhenRoleDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByNameAsync("DataManager");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task AddRoleAsync_ShouldAddRoleToDatabase()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };

        // Act
        await _sut.AddRoleAsync(role);

        // Assert
        Assert.Equal(1, context.Roles.Count());
        Assert.Contains(context.Roles, r => r is { Id: 1, RoleName: "Admin", RolePolicy: "gold" });
    }

    [Fact]
    public async Task DeleteRole_ShouldRemoveRoleWithInputRoleNameAndReturnTrue_WhenRoleExists()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteRoleAsync("Admin");

        // Assert
        Assert.True(result);
        Assert.Equal(0, context.Roles.Count());
    }

    [Fact]
    public async Task DeleteRole_ShouldReturnFalse_WhenRoleDoesNotExist()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.DeleteRoleAsync("DataManager");

        // Assert
        Assert.False(result);
        Assert.Equal(1, context.Roles.Count());
    }

    [Fact]
    public async Task GetAllRolesPaginationAsync_ShouldReturnPaginatedResults_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        context.Roles.AddRange(
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" },
            new Role { Id = 2, RoleName = "DataManager", RolePolicy = "silver" },
            new Role { Id = 3, RoleName = "DataAnalyst", RolePolicy = "bronze" }
        );

        await context.SaveChangesAsync();

        var page = 0;
        var limit = 2;

        // Act
        var result = await _sut.GetAllRolesPaginationAsync(page, limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Admin", result[0].RoleName);
        Assert.Equal("DataManager", result[1].RoleName);
    }

    [Fact]
    public async Task GetRolesCountAsync_ShouldReturnCountOfRoles_Whenever()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        context.Roles.AddRange(
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" },
            new Role { Id = 2, RoleName = "DataManager", RolePolicy = "silver" },
            new Role { Id = 3, RoleName = "DataAnalyst", RolePolicy = "bronze" }
        );

        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRolesCountAsync();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task GetRolesByPolicyAsync_ShouldReturnRoles_WhenRolesExistForPolicy()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        context.Roles.AddRange(
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" },
            new Role { Id = 2, RoleName = "DataManager", RolePolicy = "silver" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRolesByPolicyAsync("gold");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1,result.Count());
        Assert.Contains("admin", result);
    }
    
    [Fact]
    public async Task GetRolesByPolicyAsync_ShouldReturnEmptyList_WhenNoRolesMatchPolicy()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = CreateDbContext();

        // Arrange
        context.Roles.AddRange(
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" }
        );
        await context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRolesByPolicyAsync("silver");

        // Assert
        Assert.Empty(result);
    }

}
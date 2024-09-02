using AnalysisData.Data;
using AnalysisData.Model;
using Microsoft.EntityFrameworkCore;

namespace TestProject.User.Repository.RoleRepository;

public class RoleRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly AnalysisData.Repository.RoleRepository.RoleRepository _sut;

    public RoleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestRoleDatabase").Options;
        _context = new ApplicationDbContext(options);
        _sut = new AnalysisData.Repository.RoleRepository.RoleRepository(_context);
    }

    private async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    private async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetRoleByIdAsync_ShouldReturnsRoleWithInputId_WhenRoleExists()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetRoleByIdAsync_ShouldReturnsNull_WhenRoleDoesNotExist()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByIdAsync(2);

        // Assert
        Assert.Null(result);
        await DisposeAsync();
    }


    [Fact]
    public async Task GetRoleByNameAsync_ShouldReturnsRoleWithInputRoleName_WhenRoleExists()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByNameAsync("Admin");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin", result.RoleName);
        await DisposeAsync();
    }

    [Fact]
    public async Task GetRoleByNameAsync_ShouldReturnsNull_WhenRoleDoesNotExist()
    {
        //Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRoleByNameAsync("DataManager");

        // Assert
        Assert.Null(result);
        await DisposeAsync();
    }

    [Fact]
    public async Task AddRoleAsync_ShouldAddsRoleToDatabase()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };

        // Act
        await _sut.AddRoleAsync(role);

        // Assert
        Assert.Equal(1, _context.Roles.Count());
        Assert.Contains(_context.Roles, r => r is { Id: 1, RoleName: "Admin", RolePolicy: "gold" });
        await DisposeAsync();
    }

    [Fact]
    public async Task DeleteRole_ShouldRemovesRoleWithInputRoleNameAndReturnsTrue_WhenRoleExists()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();


        // Act
        var result = await _sut.DeleteRoleAsync("Admin");

        // Assert
        Assert.True(result);
        Assert.Equal(0, _context.Roles.Count());
        await DisposeAsync();
    }

    [Fact]
    public async Task DeleteRole_ShouldReturnsFalse_WhenRoleDoesNotExist()
    {
        // Arrange
        await InitializeAsync();
        var role = new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" };
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();


        // Act
        var result = await _sut.DeleteRoleAsync("DataManager");

        // Assert
        Assert.False(result);
        Assert.Equal(1, _context.Roles.Count());
        await DisposeAsync();
    }

    [Fact]
    public async Task GetAllRolesPaginationAsync_ShouldReturnPaginatedResults_Whenever()
    {
        // Arrange
        await InitializeAsync();
        _context.Roles.AddRange(
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" },
            new Role { Id = 2, RoleName = "DataManager", RolePolicy = "silver" },
            new Role { Id = 3, RoleName = "DataAnalyst", RolePolicy = "boronz" }
        );

        await _context.SaveChangesAsync();

        var page = 0;
        var limit = 2;

        // Act
        var result = await _sut.GetAllRolesPaginationAsync(page, limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Admin", result[0].RoleName);
        Assert.Equal("DataManager", result[1].RoleName);
        await DisposeAsync();
    }
    
    [Fact]
    public async Task GetRolesCountAsync_ShouldReturnCountsOfRoles_Whenever()
    {
        // Arrange
        await InitializeAsync();
        _context.Roles.AddRange(
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" },
            new Role { Id = 2, RoleName = "DataManager", RolePolicy = "silver" },
            new Role { Id = 3, RoleName = "DataAnalyst", RolePolicy = "boronz" }
        );

        await _context.SaveChangesAsync();

        // Act
        var result = await _sut.GetRolesCountAsync();

        // Assert
        Assert.Equal(3, result);
        await DisposeAsync();
    }
    
    
}
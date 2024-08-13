using AnalysisData.Data;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Repository.RoleRepository;

public class RoleRepositoryTests
{
    [Fact]
    public async Task GetRole_ShouldReturnsRole_WhenRoleExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using (var context = new ApplicationDbContext(options))
        {
            context.Roles.Add(new Role { Id = 1, RoleName = "Admin" });
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(options))
        {
            var repository = new AnalysisData.Repository.RoleRepository.RoleRepository(context);

            // Act
            var result = await repository.GetRole(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Admin", result.RoleName);
        }
    }

    [Fact]
    public void AddRole_ShouldAddsRoleToDatabase_Whenever()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);
        var repository = new AnalysisData.Repository.RoleRepository.RoleRepository(context);
        var role = new Role { Id = 2, RoleName = "User" };

        // Act
        repository.AddRole(role);

        // Assert
        Assert.Equal(3, context.Roles.Count());
        Assert.Contains(context.Roles, r => r.RoleName == "User");
    }

    [Fact]
    public void DeleteRole_ShouldRemovesRole_WhenRoleExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Roles.AddRange(
            new Role { Id = 3, RoleName = "Manager" }
        );
        context.SaveChanges();

        var repository = new AnalysisData.Repository.RoleRepository.RoleRepository(context);

        // Act
        var result = repository.DeleteRole(3);

        // Assert
        Assert.True(result);
        Assert.Equal(2, context.Roles.Count());
        Assert.DoesNotContain(context.Roles, r => r.Id == 3);
    }

    [Fact]
    public void DeleteRole_ShouldReturnsFalse_WhenRoleDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(options);
        context.Roles.AddRange(
            new Role { Id = 4, RoleName = "Editor" }
        );
        context.SaveChanges();

        var repository = new AnalysisData.Repository.RoleRepository.RoleRepository(context);

        // Act
        var result = repository.DeleteRole(5);

        // Assert
        Assert.False(result);
        Assert.Equal(1, context.Roles.Count());
    }
}
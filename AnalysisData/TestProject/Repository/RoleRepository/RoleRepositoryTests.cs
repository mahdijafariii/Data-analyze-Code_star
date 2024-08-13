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
        Assert.Equal(2, context.Roles.Count());
        Assert.Contains(context.Roles, r => r.RoleName == "User");
    }
}
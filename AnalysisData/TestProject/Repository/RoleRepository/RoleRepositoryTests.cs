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
            context.Roles.Add(new Role { Id = 2, RoleName = "User" });
            await context.SaveChangesAsync();
        }

        await using (var context = new ApplicationDbContext(options))
        {
            var repository = new AnalysisData.Repository.RoleRepository.RoleRepository(context);

            // Act
            var result = await repository.GetRole(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User", result.RoleName);
        }
    }
}
// using AnalysisData.Data;
// using AnalysisData.UserManage.Model;
// using Microsoft.EntityFrameworkCore;
//
// namespace TestProject.Repository.RoleRepository;
//
// public class RoleRepositoryTests
// {
//     private readonly DbContextOptions<ApplicationDbContext> _options;
//     private readonly ApplicationDbContext _context;
//     private readonly AnalysisData.Repository.RoleRepository.RoleRepository _sut;
//
//     public RoleRepositoryTests()
//     {
//         _options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase("TestDatabase").Options;
//         _context = new ApplicationDbContext(_options);
//         _sut = new AnalysisData.Repository.RoleRepository.RoleRepository(_context);
//     }
//
//     [Fact]
//     public async Task GetRole_ShouldReturnsRole_WhenRoleExists()
//     {
//         // Arrange
//         var role = new Role { Id = 1, RoleName = "Admin" };
//         _context.Roles.Add(role);
//         await _context.SaveChangesAsync();
//
//         // Act
//         var result = await _sut.GetRoleById(1);
//
//         // Assert
//         Assert.NotNull(result);
//         Assert.Equal("Admin", result.RoleName);
//         _context.Roles.Remove(role);
//         _context.SaveChangesAsync();
//     }
//
//     [Fact]
//     public void AddRole_ShouldAddsRoleToDatabase_Whenever()
//     {
//         // Arrange
//         var role = new Role { Id = 1, RoleName = "Admin" };
//
//         // Act
//         _sut.AddRole(role);
//
//         // Assert
//         Assert.Equal(1, _context.Roles.Count());
//         Assert.Contains(_context.Roles, r => r.RoleName == "Admin");
//         _context.Roles.Remove(role);
//         _context.SaveChangesAsync();
//     }
//
//     [Fact]
//     public void DeleteRole_ShouldRemovesRole_WhenRoleExists()
//     {
//         // Arrange
//         var role = new Role { Id = 1, RoleName = "Admin" };
//         _context.Roles.Add(role);
//         _context.SaveChanges();
//
//         // Act
//         var result = _sut.DeleteRole("Admin");
//         // Assert
//         Assert.True(result);
//         Assert.Equal(0, _context.Roles.Count());
//         Assert.DoesNotContain(_context.Roles, r => r.Id == 1);
//         _context.Roles.Remove(role);
//         _context.SaveChangesAsync();
//     }
//
//     [Fact]
//     public void DeleteRole_ShouldReturnsFalse_WhenRoleDoesNotExist()
//     {
//         // Arrange
//         var role = new Role { Id = 1, RoleName = "Editor" };
//         _context.Roles.Add(role);
//         _context.SaveChanges();
//
//         // Act
//         var result = _sut.DeleteRole("admin");
//
//         // Assert
//         Assert.False(result);
//         Assert.Equal(1, _context.Roles.Count());
//         _context.Roles.Remove(role);
//         _context.SaveChangesAsync();
//     }
// }
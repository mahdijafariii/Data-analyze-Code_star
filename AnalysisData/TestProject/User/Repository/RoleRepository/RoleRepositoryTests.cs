// using AnalysisData.Model;
// using Microsoft.EntityFrameworkCore;
// using NSubstitute;
// using System.Linq.Expressions;
// using AnalysisData.Data;
// using MockQueryable.FakeItEasy;
// using NSubstitute.ReturnsExtensions;
//
// namespace TestProject.User.Repository.RoleRepository
// {
//     public class RoleRepositoryTests
//     {
//         private readonly ApplicationDbContext _context;
//         private readonly DbSet<Role> _roleSet;
//         private readonly AnalysisData.Repository.RoleRepository.RoleRepository _sut;
//         private readonly IQueryable<Role> _roles;
//
//         public RoleRepositoryTests()
//         {
//             // Initialize the roles list
//             _roles = new List<Role>
//             {
//                 new Role { Id = 1, RoleName = "Admin", RolePolicy = "gold" },
//                 new Role { Id = 2, RoleName = "DataManager", RolePolicy = "silver" },
//                 new Role { Id = 3, RoleName = "DataAnalyst", RolePolicy = "bronze" }
//             }.AsQueryable();
//
//             _roleSet = _roles.BuildMockDbSet();
//
//             _context = Substitute.For<ApplicationDbContext>();
//             _context.Roles.Returns(_roleSet);
//
//             // Initialize the repository with the mock context
//             _sut = new AnalysisData.Repository.RoleRepository.RoleRepository(_context);
//         }
//
//         [Fact]
//         public async Task GetRoleByIdAsync_ShouldReturnRoleWithInputId_WhenRoleExists()
//         {
//             // Arrange
//             _roleSet.FindAsync(1).Returns(_roles.FirstOrDefault(r => r.Id == 1));
//
//             // Act
//             var result = await _sut.GetRoleByIdAsync(1);
//
//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal("Admin", result.RoleName);
//         }
//
//         [Fact]
//         public async Task GetRoleByIdAsync_ShouldReturnNull_WhenRoleDoesNotExist()
//         {
//             // Arrange
//             _roleSet.FindAsync(99).ReturnsNull();
//
//             // Act
//             var result = await _sut.GetRoleByIdAsync(99);
//
//             // Assert
//             Assert.Null(result);
//         }
//
//         [Fact]
//         public async Task GetRoleByNameAsync_ShouldReturnRoleWithInputRoleName_WhenRoleExists()
//         {
//             // Arrange
//             Expression<Func<Role, bool>> expression = role => role.RoleName == "Admin";
//             _roleSet.FirstOrDefaultAsync(expression).Returns(_roles.FirstOrDefault(expression.Compile()));
//
//             // Act
//             var result = await _sut.GetRoleByNameAsync("Admin");
//
//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal("Admin", result.RoleName);
//         }
//
//         [Fact]
//         public async Task GetRoleByNameAsync_ShouldReturnNull_WhenRoleDoesNotExist()
//         {
//             // Arrange
//             Expression<Func<Role, bool>> expression = role => role.RoleName == "NonExistingRole";
//             _roleSet.FirstOrDefaultAsync(expression).ReturnsNull();
//
//             // Act
//             var result = await _sut.GetRoleByNameAsync("NonExistingRole");
//
//             // Assert
//             Assert.Null(result);
//         }
//
//         [Fact]
//         public async Task AddRoleAsync_ShouldAddRoleToDatabase()
//         {
//             // Arrange
//             var newRole = new Role { Id = 4, RoleName = "NewRole", RolePolicy = "platinum" };
//
//             // Act
//             await _sut.AddRoleAsync(newRole);
//
//             // Assert
//             await _roleSet.Received(1).AddAsync(newRole);
//             await _context.Received(1).SaveChangesAsync();
//         }
//
//         [Fact]
//         public async Task DeleteRoleAsync_ShouldRemoveRoleWithInputRoleNameAndReturnTrue_WhenRoleExists()
//         {
//             // Arrange
//             Expression<Func<Role, bool>> expression = role => role.RoleName == "Admin";
//             var existingRole = _roles.FirstOrDefault(expression.Compile());
//             _roleSet.FirstOrDefaultAsync(expression).Returns(existingRole);
//
//             // Act
//             var result = await _sut.DeleteRoleAsync("Admin");
//
//             // Assert
//             Assert.True(result);
//             _roleSet.Received(1).Remove(existingRole);
//             await _context.Received(1).SaveChangesAsync();
//         }
//
//         [Fact]
//         public async Task DeleteRoleAsync_ShouldReturnFalse_WhenRoleDoesNotExist()
//         {
//             // Arrange
//             Expression<Func<Role, bool>> expression = role => role.RoleName == "NonExistingRole";
//             _roleSet.FirstOrDefaultAsync(expression).ReturnsNull();
//
//             // Act
//             var result = await _sut.DeleteRoleAsync("NonExistingRole");
//
//             // Assert
//             Assert.False(result);
//             _roleSet.DidNotReceive().Remove(Arg.Any<Role>());
//             await _context.DidNotReceive().SaveChangesAsync();
//         }
//
//         [Fact]
//         public async Task GetAllRolesPaginationAsync_ShouldReturnPaginatedResults()
//         {
//             // Arrange
//             var paginatedRoles = _roles.Take(2).AsQueryable();
//             _roleSet.AsQueryable().Returns(paginatedRoles);
//
//             // Act
//             var result = await _sut.GetAllRolesPaginationAsync(0, 2);
//
//             // Assert
//             Assert.NotNull(result);
//             Assert.Equal(2, result.Count);
//             Assert.Equal("Admin", result[0].RoleName);
//             Assert.Equal("DataManager", result[1].RoleName);
//         }
//
//         [Fact]
//         public async Task GetRolesCountAsync_ShouldReturnCountOfRoles()
//         {
//             // Arrange
//             _roleSet.CountAsync().Returns(_roles.Count());
//
//             // Act
//             var result = await _sut.GetRolesCountAsync();
//
//             // Assert
//             Assert.Equal(3, result);
//         }
//     }
// }
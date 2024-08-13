using AnalysisData.Data;
using AnalysisData.UserManage.Model;
using Microsoft.EntityFrameworkCore;

namespace TestProject.Repository.UserRoleRepository;

public class UserRoleRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly AnalysisData.Repository.UserRoleRepository.UserRoleRepository _sut;

    public UserRoleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _sut = new AnalysisData.Repository.UserRoleRepository.UserRoleRepository(_context);
    }

    [Fact]
    public void Add_ShouldAddUserRoleToDatabase_WhenAddNewUser()
    {
        // Arrange
        var userRole = new UserRole { UserId = 1, RoleId = 1 };

        // Act
        _sut.Add(userRole);

        // Assert
        Assert.Equal(1, _context.UserRoles.Count());
        _context.UserRoles.Remove(userRole);
        _context.SaveChanges();

    }

    [Fact]
    public void DeleteUserInUserRole_ShouldRemoveAllRolesOfUser_WhenRemoveUser()
    {
        // Arrange
        _context.UserRoles.Add(new UserRole { UserId = 1, RoleId = 2 });
        _context.UserRoles.Add(new UserRole { UserId = 1, RoleId = 3 });
        _context.UserRoles.Add(new UserRole { UserId = 2, RoleId = 3 });
        _context.SaveChanges();

        // Act
        var result = _sut.DeleteUserInUserRole(1);

        // Assert
        Assert.True(result);
        Assert.Equal(1, _context.UserRoles.Count());
        _context.UserRoles.RemoveRange(_context.UserRoles.ToList());
        _context.SaveChangesAsync();
    }

    [Fact]
    public void DeleteRoleInUserRole_ShouldRemoveAllUsersWithRole_WhenRemoveRole()
    {
        // Arrange
        _context.UserRoles.Add(new UserRole { UserId = 1, RoleId = 2 });
        _context.UserRoles.Add(new UserRole { UserId = 2, RoleId = 2 });
        _context.UserRoles.Add(new UserRole { UserId = 1, RoleId = 3 });
        _context.SaveChanges();

        // Act
        var result = _sut.DeleteRoleInUserRole(2);

        // Assert
        Assert.True(result);
        Assert.Equal(1, _context.UserRoles.Count());
        _context.UserRoles.RemoveRange(_context.UserRoles.ToList());
        _context.SaveChangesAsync();
    }
}
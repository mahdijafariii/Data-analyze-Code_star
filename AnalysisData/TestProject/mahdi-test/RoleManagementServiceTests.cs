using AnalysisData.Exception;
using AnalysisData.Exception.UserException;

using AnalysisData.User.Model;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using AnalysisData.User.Services.RoleService;
using AnalysisData.User.UserDto.RoleDto;
using NSubstitute;

namespace TestProject.mahdi_test;

public class RoleManagementServiceTests
{
    private readonly IRoleRepository _roleRepositoryMock;
    private readonly RoleManagementService _sut;

    public RoleManagementServiceTests()
    {
        _roleRepositoryMock = Substitute.For<IRoleRepository>();
        _sut = new RoleManagementService(_roleRepositoryMock);
    }


    [Fact]
    public async Task GetRoleCount_ShouldReturnsCorrectCount_WhenHaveManyRolesInRepository()
    {
        // Arrange
        int expectedCount = 5;
        _roleRepositoryMock.GetRolesCountAsync()
            .Returns(expectedCount);

        // Act
        int result = await _sut.GetRoleCount();

        // Assert
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public async Task GetRoleCount_ShouldReturnReturnsZero_WhenNoRolesExist()
    {
        // Arrange
        int expectedCount = 0;
        _roleRepositoryMock.GetRolesCountAsync()
            .Returns(expectedCount);

        // Act
        int result = await _sut.GetRoleCount();

        // Assert
        Assert.Equal(expectedCount, result);
    }

    [Fact]
    public async Task GetRolePagination_ShouldReturnsCorrectPagedRoles_WhenMultipleRolesExist()
    {
        // Arrange
        int page = 1;
        int limit = 10;
        var roles = new List<Role>
        {
            new Role { Id = 1, RoleName = "Admin", RolePolicy = "AdminPolicy" },
            new Role { Id = 2, RoleName = "User", RolePolicy = "UserPolicy" }
        };

        _roleRepositoryMock.GetAllRolesPaginationAsync(page, limit)
            .Returns(Task.FromResult(roles));

        var expectedRoles = roles.Select(x => new RolePaginationDto
        {
            Id = x.Id.ToString(),
            Name = x.RoleName,
            Policy = x.RolePolicy
        }).ToList();

        // Act
        var result = await _sut.GetRolePagination(page, limit);

        // Assert 
        Assert.Equivalent(expectedRoles, result);
    }
    
    
    [Fact]
    public async Task GetRolePagination_ShouldReturnsCorrectPagedRoles_WhenNoRolesExist()
    {
        // Arrange
        int page = 1;
        int limit = 10;
        var roles = new List<Role>();

        _roleRepositoryMock.GetAllRolesPaginationAsync(page, limit)
            .Returns(Task.FromResult(roles));

        var expectedRoles = roles.Select(x => new RolePaginationDto
        {
            Id = x.Id.ToString(),
            Name = x.RoleName,
            Policy = x.RolePolicy
        }).ToList();

        // Act
        var result = await _sut.GetRolePagination(page, limit);

        // Assert 
        Assert.Equivalent(expectedRoles, result);
    }
    
    [Fact]
    public async Task DeleteRole_ShouldDeleteRole_WhenRoleExists()
    {
        // Arrange
        string roleName = "Admin";
        var existingRole = new Role { RoleName = roleName };
        _roleRepositoryMock.GetRoleByNameAsync(roleName).Returns(existingRole);

        // Act
        await _sut.DeleteRole(roleName);

        // Assert
        await _roleRepositoryMock.Received(1).DeleteRoleAsync(roleName);
    }

    [Fact]
    public async Task DeleteRole_ShouldThrowRoleNotFoundException_WhenRoleDoesNotExist()
    {
        // Arrange
        string roleName = "NonExistentRole";
        _roleRepositoryMock.GetRoleByNameAsync(roleName).Returns((Role)null);

        // Act & Assert
        await Assert.ThrowsAsync<RoleNotFoundException>(() => _sut.DeleteRole(roleName));
    }
    
    [Fact]
    public async Task AddRole_ShouldNotAddRole_WhenRoleExistsBefore()
    {
        // Arrange
        string roleName = "admin";
        var existingRole = new Role { RoleName = roleName  , RolePolicy = "gold"};
        _roleRepositoryMock.GetRoleByNameAsync(roleName).Returns(existingRole);
        
        // Assert && Act
        await Assert.ThrowsAsync<DuplicateRoleExistException>(() => _sut.AddRole(roleName, "gold"));
    }

    [Fact]
    public async Task AddRole_ShouldAddRole_WhenRoleNotExistsBefore()
    {
        // Arrange
        string roleName = "Admin";
        string rolePolicy = "Gold";

        var newRole = new Role { RoleName = roleName.ToLower(), RolePolicy = rolePolicy.ToLower() };

        _roleRepositoryMock.GetRoleByNameAsync(roleName).Returns((Role)null);

        // Act
        await _sut.AddRole(roleName, rolePolicy);


        // Assert
        await _roleRepositoryMock.Received(1).AddRoleAsync(Arg.Is<Role>(r => 
            r.RoleName == roleName.ToLower() && r.RolePolicy == rolePolicy.ToLower()));    }
}
using AnalysisData.User.Controllers;
using AnalysisData.User.Services.PermissionService.Abstraction;
using AnalysisData.User.Services.RoleService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.UserDto.RoleDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSubstitute;

namespace TestProject.User.Controllers;

public class RoleControllerTests
{
    private readonly IRoleManagementService _roleManagementService;
    private readonly RoleController _sut;
    
    public RoleControllerTests()
    {
        _roleManagementService = Substitute.For<IRoleManagementService>();
        _sut = new RoleController(_roleManagementService);
    }
    
    [Fact]
    public async Task DeleteRole_ShouldReturnOk_WhenRoleDeletedSuccessfully()
    {
        // Arrange
        var roleName = "admin";
        _roleManagementService.DeleteRole(roleName).Returns(Task.CompletedTask);
    
        // Act
        var result = await _sut.DeleteRole(roleName);
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = "Role deleted successfully." });
        Assert.Equal(expectedResponseContent, responseContent);

        await _roleManagementService.Received(1).DeleteRole(roleName);
    }
    
    [Fact]
    public async Task AddRole_ShouldReturnOk_WhenRoleAddedSuccessfully()
    {
        // Arrange
        var roleDto = new AddRoleDto { Name = "admin", Policy = "policy" };
        _roleManagementService.AddRole(roleDto.Name, roleDto.Policy).Returns(Task.CompletedTask);
    
        // Act
        var result = await _sut.AddRole(roleDto);
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new { message = "Role added successfully." });
        Assert.Equal(expectedResponseContent, responseContent);

        await _roleManagementService.Received(1).AddRole(roleDto.Name, roleDto.Policy);
    }

    [Fact]
    public async Task GetAllRoles_ShouldReturnOk_WithPaginationData()
    {
        // Arrange
        var roles = new List<RolePaginationDto>
        {
            new RolePaginationDto(){Name = "admin",Policy = "gold"},
            new RolePaginationDto(){Name = "dataanalyst",Policy = "silver"}
        };
        var page = 0;
        var limit = 10;
        var rolesCount = 2;

        _roleManagementService.GetRolePagination(page, limit).Returns(roles);
        _roleManagementService.GetRoleCount().Returns(rolesCount);
    
        // Act
        var result = await _sut.GetAllRoles(page, limit);
    
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseContent = JsonConvert.SerializeObject(okResult.Value);
        var expectedResponseContent = JsonConvert.SerializeObject(new
        {
            roles,
            count = rolesCount,
            thisPage = page
        });
        Assert.Equal(expectedResponseContent, responseContent);

        await _roleManagementService.Received(1).GetRolePagination(page, limit);
        await _roleManagementService.Received(1).GetRoleCount();
    }



}
using AnalysisData.Services;
using AnalysisData.Services.RoleService.Abstraction;
using AnalysisData.UserDto.RoleDto;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IRoleManagementService _roleManagementService;

    public RoleController(IRoleManagementService roleManagementService)
    {
        _roleManagementService = roleManagementService;
    }


    [HttpDelete("{roleName}")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        await _roleManagementService.DeleteRole(roleName);
        return Ok(new { message = "Role deleted successfully." });
    }

    [HttpPost]
    public async Task<IActionResult> AddRole([FromBody] AddRoleDto role)
    {
        await _roleManagementService.AddRole(role.Name, role.Policy);
        return Ok(new { message = "Role added successfully." });
    }


    [HttpGet]
    public async Task<IActionResult> GetAllRoles(int page = 0, int limit = 10)
    {
        var rolesPagination = await _roleManagementService.GetRolePagination(page, limit);
        var rolesCount = await _roleManagementService.GetRoleCount();
        return Ok(new
        {
            users = rolesPagination,
            count = rolesCount,
            thisPage = page,
        });
    }
}
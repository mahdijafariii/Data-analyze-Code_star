using AnalysisData.Services;
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

    
    [HttpDelete("DeleteRole")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        await _roleManagementService.DeleteRole(roleName);
        return Ok(new { message = "Role deleted successfully." });
    }

    [HttpPost("AddRole")]
    public async Task<IActionResult> AddRole(string name , string policy)
    {
        await _roleManagementService.AddRole(name, policy);
        return Ok(new { message = "Role added successfully." });
    }
    
     
    [HttpGet("GetRolesPagination")]
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
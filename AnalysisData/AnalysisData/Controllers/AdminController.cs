using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;

[ApiController]
//[Authorize(Roles = "admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    //[Authorize(Roles = "admin")]
    [HttpGet("GetUsers")]
    public IActionResult GetAllUsers()
    {
        var users = _adminService.GetAllUsers().Result;

        if (users == null || users.Count == 0)
        {
            return NoContent();
        }

        return Ok(users);
    }

    [HttpGet("GetRoles")]
    public IActionResult GetAllRoles()
    {
        var roles = _adminService.GetAllRoles().Result;

        if (roles == null || roles.Count == 0)
        {
            return NoContent();
        }

        return Ok(roles);
    }
    [HttpDelete("DeleteUser/{username}")]
    public IActionResult DeleteUser(string username)
    {
        var result = _adminService.DeleteUser(username);
        if (result)
        {
            return Ok(new { message = "User deleted successfully." });
        }

        return NotFound(new { message = "User not found." });
    }

    [HttpDelete("DeleteRole/{roleName}")]
    public IActionResult DeleteRole(string roleName)
    {
        var result = _adminService.DeleteRole(roleName);
        if (result)
        {
            return Ok(new { message = "Role deleted successfully." });
        }
    
        return NotFound(new { message = "Role not found." });
    }

    [HttpPost("AddRole/{roleName}")]
    public IActionResult AddRole(string roleName)
    {
        var result = _adminService.AddRole(roleName);
        if (result)
        {
            return Ok("success");
        }

        return BadRequest("not success");
    }
    
    
}
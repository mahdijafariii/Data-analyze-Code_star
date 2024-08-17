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
    
    
}
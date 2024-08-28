using AnalysisData.Services;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.UpdateModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;

[ApiController]
//[Authorize(Roles = "admin")]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IAdminRegisterService _adminRegisterService;

    public AdminController(IAdminService adminService,IAdminRegisterService adminRegisterService)
    {
        _adminService = adminService;
        _adminRegisterService = adminRegisterService;
    }

    [Authorize(Roles = "admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
    {
        await _adminRegisterService.RegisterByAdminAsync(userRegisterModel);
        return Ok(new {massage = "User added successfully"});
    }

    //[Authorize(Roles = "admin")]
    [HttpGet("GetUsersPagination")]
    public async Task<IActionResult> GetAllUsers(int page = 0, int limit = 10)
    {
        var usersPagination = await _adminService.GetAllUserAsync(page, limit);
        var userCount = await _adminService.GetUserCountAsync();
        return Ok(new
        {
            users = usersPagination,
            count = userCount,
            thisPage = page,
        });
    }
   

    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var isDeleted = await _adminService.DeleteUserAsync(id);
        if (isDeleted)
        {
            return Ok(new { message = "User deleted successfully." });
        }

        return NotFound(new { message = "User not found." });
    }
    
    [Authorize(Roles = "admin")]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateAdminModel updateAdminModel)
    {
      await _adminService.UpdateUserInformationByAdminAsync(id, updateAdminModel);
      return Ok(new {massage = "updated successfully"});
    }

    [HttpGet("firstAdmin")]
    [AllowAnonymous]
    public async Task<IActionResult> FirstAdmin()
    {
        await _adminRegisterService.AddFirstAdminAsync();
        return Ok(new { message = "success" });
    }
}
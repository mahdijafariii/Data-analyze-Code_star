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

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    //[Authorize(Roles = "admin")]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
    {
        var isRegister = await _adminService.Register(userRegisterModel);
        if (isRegister)
        {
            return Ok(new {massage = "User added successfully"});
        }

        return BadRequest("not success");
    }

    //[Authorize(Roles = "admin")]
    [HttpGet("GetUsersPagination")]
    public async Task<IActionResult> GetAllUsers(int page = 1, int limit = 10)
    {
        var usersPagination = await _adminService.GetUserPagination(page, limit);
        var userCount = await _adminService.GetUserCount();
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
        var isDeleted = await _adminService.DeleteUser(id);
        if (isDeleted)
        {
            return Ok(new { message = "User deleted successfully." });
        }

        return NotFound(new { message = "User not found." });
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateAdminModel updateAdminModel)
    {
        var isUpdated = await _adminService.UpdateUserInformationByAdmin(id, updateAdminModel);
        if (isUpdated)
        {
            return Ok(new { message = "success" });
        }

        return BadRequest(new { message ="not success"});
    }

    [HttpGet("firstAdmin")]
    [AllowAnonymous]
    public async Task<IActionResult> FirstAdmin()
    {
        await _adminService.AddFirstAdmin();
        return Ok(new { message = "success" });
    }
}
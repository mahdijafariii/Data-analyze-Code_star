using System.Security.Claims;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.ResetPasswordModel;
using AnalysisData.UserManage.UpdateModel;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;

    public UserController(IUserService userService, IPermissionService permissionService)
    {
        _userService = userService;
        _permissionService = permissionService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginModel userLoginModel)
    {
        var user = _userService.Login(userLoginModel);
        return Ok(new { user.Result.FirstName, user.Result.LastName, user.Result.ImageURL });
    }
    
    [HttpGet("permissions")]
    public IActionResult GetPermissions()
    {
        var userClaims = User;
        var permission = _permissionService.GetPermission(userClaims);
        var username = userClaims.FindFirstValue("username");
        var firstName = userClaims.FindFirstValue("firstname");
        var lastName = userClaims.FindFirstValue("lastname");
        return Ok(new { username, firstName, lastName, permission });
    }


    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
    {
        var userClaim = User;
        var check = await _userService.ResetPassword(userClaim, resetPasswordModel.NewPassword,
            resetPasswordModel.ConfirmPassword);
        if (check)
        {
            return Ok("success");
        }

        return BadRequest("not success");
    }
    
    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser(Guid id, [FromBody] UpdateUserModel updateUserModel)
    {
        var updatedUser = _userService.UpdateUserInformationByUser(id, updateUserModel);
        if (updatedUser!=null)
        {
            return Ok("success");
        }
    
        return BadRequest("not success");
    }
}
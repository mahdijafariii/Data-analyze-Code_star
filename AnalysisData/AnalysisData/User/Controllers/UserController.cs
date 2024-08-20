using System.Security.Claims;
using AnalysisData.Exception;
using AnalysisData.Services;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.NewPasswordModel;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.ResetPasswordModel;
using AnalysisData.UserManage.UpdateModel;
using Microsoft.AspNetCore.Authorization;
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
        var user = _userService.Login(userLoginModel).Result;
        return Ok(new { user.FirstName, user.LastName, user.ImageURL });
    }

    [HttpGet("permissions")]
    public IActionResult GetPermissions()
    {
        var userClaims = User;
        var permission = _permissionService.GetPermission(userClaims);
        var firstName = userClaims.FindFirstValue("firstname");
        var lastName = userClaims.FindFirstValue("lastname");
        var image = userClaims.FindFirstValue("image");

        return Ok(new { image, firstName, lastName, permission });
    }

    [Authorize(Roles = "admin")]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPasswordModel)
    {
        var userClaim = User;
        var check = await _userService.ResetPassword(userClaim, resetPasswordModel.NewPassword,
            resetPasswordModel.ConfirmPassword);
        if (check)
        {
            return Ok(new {massage = "success"});
        }

        return BadRequest(new {massage = "not success"});
    }

    [Authorize(Roles = "admin")]
    [HttpPost("UploadImage")]
    public IActionResult UploadImage(IFormFile file)
    {
        var user = User;
        if (file == null || file.Length == 0)
        {
            return BadRequest(new {massage = "No file uploaded."});
        }

        _userService.UploadImage(user, file.FileName);

        return Ok(new {massage = "Uploaded successfully."});
    }

    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser([FromBody] UpdateUserModel updateUserModel)
    {
        var user = User;
        var updatedUser = _userService.UpdateUserInformationByUser(user, updateUserModel);
        if (updatedUser != null)
        {
            return Ok(new {massage = "updated successfully"});
        }

        return BadRequest(new {massage = "not success"});
    }

    [HttpPost("new-password")]
    public async Task<IActionResult> NewPassword([FromBody] NewPasswordModel newPasswordModel)
    {
        var userClaim = User;
        var check = await _userService.NewPassword(userClaim, newPasswordModel.OldPassword,
            newPasswordModel.NewPassword,
            newPasswordModel.ConfirmPassword);
        if (check)
        {
            return Ok(new {massage = "reset successfully"});
        }

        return BadRequest(new {massage = "not success"});
    }


    [HttpGet("log-out")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok(new { message = "Logout successful" });
    }
}
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
    
    [HttpPost("UploadImage")]
    public IActionResult UploadImage(Guid id,IFormFile file)
    {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
    
            // _userService.UploadImage(id, file.FileName);
        
            return Ok("Uploaded successfully.");
    }
    
    [HttpPut("UpdateUser")]
    public IActionResult UpdateUser([FromBody] UpdateUserModel updateUserModel)
    {
        var user = User;
        var updatedUser = _userService.UpdateUserInformationByUser(user, updateUserModel);
        if (updatedUser!=null)
        {
            return Ok("success");
        }
    
        return BadRequest("not success");
    }
    
    [HttpPost("new-password")]
    public async Task<IActionResult> NewPassword([FromBody] NewPasswordModel newPasswordModel)
    {
        var userClaim = User;
        var check = await _userService.NewPassword(userClaim, newPasswordModel.OldPassword,newPasswordModel.NewPassword,
            newPasswordModel.ConfirmPassword);
        if (check)
        {
            return Ok("success");
        }

        return BadRequest("not success");
    }
    
    
    [HttpGet("log-out")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok(new { message = "Logout successful" });
    }
    
}
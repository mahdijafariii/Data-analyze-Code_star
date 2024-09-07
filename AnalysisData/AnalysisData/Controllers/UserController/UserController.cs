using System.Security.Claims;
using AnalysisData.Dtos.UserDto.PasswordDto;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Services.PermissionService.Abstraction;
using AnalysisData.Services.UserService.UserService.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers.UserController;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;
    private readonly IUploadImageService _uploadImageService;
    private readonly IResetPasswordRequestService _resetPasswordRequestService;

    public UserController(IUserService userService, IPermissionService permissionService,
        IUploadImageService uploadImageService, IResetPasswordRequestService resetPasswordRequestService)
    {
        _userService = userService;
        _permissionService = permissionService;
        _uploadImageService = uploadImageService;
        _resetPasswordRequestService = resetPasswordRequestService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        var user = await _userService.LoginAsync(userLoginDto);
        return Ok(new { user.FirstName, user.LastName, user.ImageURL });
    }

    [Authorize(Policy = "bronze")]
    [HttpGet("permissions")]
    public async Task<IActionResult> GetPermissions()
    {
        var userClaims = User;
        var permission = await _permissionService.GetPermission(userClaims);
        var firstName = userClaims.FindFirstValue("firstname");
        var lastName = userClaims.FindFirstValue("lastname");
        var image = userClaims.FindFirstValue("image");

        return Ok(new { image, firstName, lastName, permission });
    }
    
    [Authorize(Policy = "bronze")]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var userClaim = User;
        await _userService.ResetPasswordAsync(userClaim, resetPasswordDto.NewPassword,
            resetPasswordDto.ConfirmPassword, resetPasswordDto.ResetPasswordToken);
        return Ok(new { massage = "success" });
    }

    [Authorize(Policy = "bronze")]
    [HttpPost("request-reset-password")]
    public async Task<IActionResult> RequestResetPassword(EmailForResetPasswordDto resetPassword)
    {
        await _resetPasswordRequestService.SendRequestToResetPassword(resetPassword.Email);
        return Ok(new { massage = "success" });
    }
    
    [Authorize(Policy = "bronze")]
    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        var user = User;
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { massage = "No file uploaded." });
        }

        await _uploadImageService.UploadImageAsync(user, file);

        return Ok(new { massage = "Uploaded successfully." });
    }

    [Authorize(Policy = "gold")]
    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
    {
        var user = User;
        await _userService.UpdateUserInformationAsync(user, updateUserDto);
        return Ok(new { massage = "updated successfully" });
    }
    
    [Authorize(Policy = "bronze")]
    [HttpPost("new-password")]
    public async Task<IActionResult> NewPassword([FromBody] NewPasswordDto newPasswordDto)
    {
        var userClaim = User;
        await _userService.NewPasswordAsync(userClaim, newPasswordDto.OldPassword,
            newPasswordDto.NewPassword,
            newPasswordDto.ConfirmPassword);
        return Ok(new { massage = "reset successfully" });
    }

    [Authorize(Policy = "gold")]
    [HttpGet("get-user-information")]
    public async Task<IActionResult> GetUserInformation()
    {
        var user = User;
        var result = await _userService.GetUserAsync(user);
        if (result != null)
        {
            return Ok(new GetUserInformationDto()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                Email = result.Email,
                Image = result.ImageURL ?? "User do not have information yet !"
            });
        }

        return BadRequest(new { message = "not found!" });
    }

    [HttpPost("logOut")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("AuthToken");
        return Ok(new { message = "Logout successful" });
    }
}
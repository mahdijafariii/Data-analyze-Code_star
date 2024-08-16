using System.Security.Claims;
using AnalysisData.Exception;
using AnalysisData.Services;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.RegisterModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;

    public UserController(IUserService userService,IPermissionService permissionService)
    {
        _userService = userService;
        _permissionService = permissionService;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginModel userLoginModel)
    {
        var user = _userService.Login(userLoginModel);
        return Ok(new { user.Result.FirstName , user.Result.LastName , user.Result.ImageURL });
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
    {
        var check =  await _userService.Register(userRegisterModel);
        if (check)
        {
            return Ok("success");
        }

        return NotFound("not success");
    }
    
    
    [HttpGet("permissions")]
    public IActionResult GetPermissions()
    {
        var userClaims = User;
        var permission = _permissionService.GetPermission(userClaims);
        return Ok(new { permission });
    }
}
using AnalysisData.Services;
using AnalysisData.UserManage.LoginModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginModel userLoginModel)
    {
        var userRoles = _userService.Login(userLoginModel);
        return Ok(new { roles = userRoles });
    }
    
    [HttpGet("login2")]
    public IActionResult Login2()
    {
        return Ok(new { massage = "salam" });
    }
}
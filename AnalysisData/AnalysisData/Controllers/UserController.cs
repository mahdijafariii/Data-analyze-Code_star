using AnalysisData.Services;
using AnalysisData.UserManage.LoginModel;
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
    [ValidateAntiForgeryToken]
    public IActionResult Login([FromBody] UserLoginModel userLoginModel)
    {
        var userRoles = _userService.Login(userLoginModel);
        return Ok(new { roles = userRoles });
    }
}
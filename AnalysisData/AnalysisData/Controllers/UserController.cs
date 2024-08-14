using AnalysisData.Services;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.RegisterModel;
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

    [HttpPost("register")]
    public IActionResult Register([FromBody] UserRegisterModel userRegisterModel)
    {
        var check =  _userService.Register(userRegisterModel);
        if (check.Result )
        {
            return Ok("success");
        }

        return Ok("not success");
    }
}
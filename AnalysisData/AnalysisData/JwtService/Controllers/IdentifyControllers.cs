using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnalysisData.UserManage.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService)
    {
        _jwtService = jwtService;
    }
    [HttpPost("token")]
    public async Task<IActionResult> GenerateToken([FromBody] string username)
    {
        var token =  _jwtService.GenerateJwtToken(username);
        return Ok(new { Token = token });
    }

}
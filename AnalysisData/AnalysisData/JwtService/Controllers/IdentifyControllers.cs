using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.JwtService.Controllers;

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
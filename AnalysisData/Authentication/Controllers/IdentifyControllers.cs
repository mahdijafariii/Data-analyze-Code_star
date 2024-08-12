using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnalysisData.UserManage.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;

public class IdentifyControllers : ControllerBase
{
    // private static readonly TimeSpan TokenLifeTime = TimeSpan.FromMinutes(10);
    // [HttpPost]
    // public IActionResult GenerateToken([FromBody] JwtService request)
    // {
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     // var key = Encoding.UTF8.GetBytes();
    //     
    //
    //
    //
    //
    //
    // }
    // [HttpPost("login")]
    // public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    // {
    //     var user = await _userManager.FindByNameAsync(loginRequest.UserName);
    //     if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
    //     {
    //         var token = await _jwtService.GenerateJwtTokenAsync(user);
    //         return Ok(new { Token = token });
    //     }
    //     
    //     return Unauthorized();
    // }
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
        public async Task<IActionResult> GenerateToken([FromBody] User loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.UserName))
            {
                return BadRequest("Username is required");
            }

            var token =  _jwtService.GenerateJwtToken(loginDto.UserName);
            return Ok(new { Token = token });
        }
    }
    
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnalysisData.RoleRepository;
using AnalysisData.UserRepositories.Abstraction;
using Microsoft.IdentityModel.Tokens;

namespace Authentication;

public class JwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;


    public JwtService(IConfiguration configuration , IUserRepository userRepository,IRoleRepository roleRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public string GenerateJwtToken(string userName)
    {
        var roles = _userRepository.GetUser(userName).Result.UserRole.ToString();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        foreach (var role in roles)
        {
            var result = _roleRepository.GetRoles(role);
            claims.Add(new Claim(ClaimTypes.Role, result.Result.RoleType));
        }
        

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}
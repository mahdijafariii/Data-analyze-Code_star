using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData.JwtService;

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

    public async Task<string> GenerateJwtToken(string userName)
    {
        var user =await _userRepository.GetUser(userName);
        var roles = user.UserRoles;
        var claims = new List<Claim>
        {
            new Claim("Name", userName),
        };
        foreach (var role in roles)
        {
            var result = await _roleRepository.GetRole(role.Id);
            claims.Add(new Claim("Roles", result.RoleName));
        }
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}
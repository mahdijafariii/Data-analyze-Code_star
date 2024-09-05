using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnalysisData.User.CookieService.abstractions;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using Microsoft.IdentityModel.Tokens;


public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;

    public JwtService(IConfiguration configuration, IUserRepository userRepository, ICookieService cookieService)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _cookieService = cookieService;
    }

    public async Task<string> GenerateJwtToken(string userName)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("firstname", user.FirstName),
            new Claim("lastname", user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Role.RoleName.ToLower()),
            new Claim("policy", user.Role.RolePolicy.ToLower()),
            new Claim("image", user.ImageURL ?? "default-image-url")
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creeds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creeds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public async Task RequestResetPassword(User user)
    {
        var token = Guid.NewGuid().ToString();
        var expiration = DateTime.UtcNow.AddMinutes(15); 

        var resetToken = new PasswordResetToken()
        {
            UserId = user.Id,
            Token = token,
            Expiration = expiration,
            IsUsed = false
        };
        
    }

    public async Task UpdateUserCookie(string userName, bool rememberMe)
    {
        var token = await GenerateJwtToken(userName);
        _cookieService.UpdateCookie("AuthToken", token, rememberMe);
    }
}
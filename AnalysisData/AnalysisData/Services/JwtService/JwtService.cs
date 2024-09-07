using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.PasswordResetTokensRepository.Abstraction;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.CookieService.Abstractions;
using AnalysisData.Services.JwtService.Abstraction;
using Microsoft.IdentityModel.Tokens;

namespace AnalysisData.Services.JwtService;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;
    private readonly IPasswordResetTokensRepository _resetTokensRepository;

    public JwtService(IConfiguration configuration, IUserRepository userRepository, ICookieService cookieService,IPasswordResetTokensRepository resetTokensRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
        _cookieService = cookieService;
        _resetTokensRepository = resetTokensRepository;
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

        await _resetTokensRepository.AddToken(resetToken);

    }

    public async Task UpdateUserCookie(string userName, bool rememberMe)
    {
        var token = await GenerateJwtToken(userName);
        _cookieService.UpdateCookie("AuthToken", token, rememberMe);
    }
}
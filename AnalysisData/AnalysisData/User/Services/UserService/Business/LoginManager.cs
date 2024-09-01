using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;

namespace AnalysisData.Services.Business;

public class LoginManager : ILoginManager
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly ICookieService _cookieService;

    public LoginManager(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService, ICookieService cookieService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _cookieService = cookieService;
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userLoginDto.userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        _passwordService.ValidatePassword(user, userLoginDto.password);
        var token = await _jwtService.GenerateJwtToken(userLoginDto.userName);
        _cookieService.SetCookie("AuthToken", token, userLoginDto.rememberMe);
        return user;
    }
}
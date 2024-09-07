using AnalysisData.Exception.UserException;
using AnalysisData.User.CookieService.abstractions;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService.Business;

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

    public async Task<Model.User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userLoginDto.UserName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        _passwordService.ValidatePassword(user, userLoginDto.Password);
        var token = await _jwtService.GenerateJwtToken(userLoginDto.UserName);
        _cookieService.SetCookie("AuthToken", token, userLoginDto.RememberMe);
        return user;
    }
}
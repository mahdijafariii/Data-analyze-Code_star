using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Exception.UserException;
using AnalysisData.Models.UserModel;
using AnalysisData.Repositories.UserRepository.Abstraction;
using AnalysisData.Services.CookieService.Abstractions;
using AnalysisData.Services.JwtService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;

namespace AnalysisData.Services.UserService.UserService.Business;

public class LoginManager : ILoginManager
{
    private readonly IUserRepository _userRepository;
    private readonly IValidtionPasswordManager _validtionPasswordManager;
    private readonly IJwtService _jwtService;
    private readonly ICookieService _cookieService;

    public LoginManager(IUserRepository userRepository, IValidtionPasswordManager validtionPasswordManager, IJwtService jwtService, ICookieService cookieService)
    {
        _userRepository = userRepository;
        _validtionPasswordManager = validtionPasswordManager;
        _jwtService = jwtService;
        _cookieService = cookieService;
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userLoginDto.UserName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
        _validtionPasswordManager.ValidatePassword(user, userLoginDto.Password);
        var token = await _jwtService.GenerateJwtToken(userLoginDto.UserName);
        _cookieService.SetCookie("AuthToken", token, userLoginDto.RememberMe);
        return user;
    }
}
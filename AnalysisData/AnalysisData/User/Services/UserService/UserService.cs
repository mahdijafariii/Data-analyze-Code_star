using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.User.CookieService.abstractions;
using AnalysisData.User.JwtService.abstractions;
using AnalysisData.User.Repository.UserRepository.Abstraction;
using AnalysisData.User.Services.SecurityPasswordService.Abstraction;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.ValidationService.Abstraction;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;
    private readonly IJwtService _jwtService;
    private readonly IValidationService _validationService;
    private readonly IPasswordHasher _passwordHasher;


    public UserService(IUserRepository userRepository, ICookieService cookieService,
        IJwtService jwtService, IValidationService validationService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _cookieService = cookieService;
        _jwtService = jwtService;
        _validationService = validationService;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _validationService.PasswordCheck(password);
        user.Password = _passwordHasher.HashPassword(password);
        await _userRepository.UpdateUserAsync(user.Id, user);
        return true;
    }

    public async Task<bool> NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password,
        string confirmPassword)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Password == _passwordHasher.HashPassword(oldPassword))
        {
            throw new PasswordMismatchException();
        }

        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _validationService.PasswordCheck(password);
        user.Password = _passwordHasher.HashPassword(password);
        await _userRepository.UpdateUserAsync(user.Id, user);
        return true;
    }

    public async Task<Model.User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userLoginDto.Username);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Password != _passwordHasher.HashPassword(userLoginDto.Password))
        {
            throw new PasswordMismatchException();
        }

        var token = await _jwtService.GenerateJwtToken(userLoginDto.Username);
        _cookieService.SetCookie("AuthToken", token, userLoginDto.RememberMe);
        return user;
    }

    public async Task<Model.User> GetUserAsync(ClaimsPrincipal userClaim)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }

    public async Task<bool> UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        var checkEmail = await _userRepository.GetUserByEmailAsync(updateUserDto.Email);

        if (checkEmail != null && user.Email != updateUserDto.Email)
            throw new DuplicateUserException();

        _validationService.EmailCheck(updateUserDto.Email);
        _validationService.PhoneNumberCheck(updateUserDto.PhoneNumber);
        await ReplaceUserDetails(user, updateUserDto);
        await _jwtService.UpdateUserCookie(userName, false);
        return true;
    }

    private async Task ReplaceUserDetails(Model.User user, UpdateUserDto updateUserDto)
    {
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        await _userRepository.UpdateUserAsync(user.Id, user);
    }
    
}
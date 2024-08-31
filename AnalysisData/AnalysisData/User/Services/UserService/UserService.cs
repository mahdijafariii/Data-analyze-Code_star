using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services;

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

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(userLoginDto.userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Password != _passwordHasher.HashPassword(userLoginDto.password))
        {
            throw new PasswordMismatchException();
        }

        var token = await _jwtService.GenerateJwtToken(userLoginDto.userName);
        _cookieService.SetCookie("AuthToken", token, userLoginDto.rememberMe);
        return user;
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal userClaim)
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

    private async Task ReplaceUserDetails(User user, UpdateUserDto updateUserDto)
    {
        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;
        user.Email = updateUserDto.Email;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        await _userRepository.UpdateUserAsync(user.Id, user);
    }

    public async Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, string imageUrl)
    {
        var userName = claimsPrincipal.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        user.ImageURL = imageUrl;
        await _userRepository.UpdateUserAsync(user.Id, user);
        return true;
    }
}
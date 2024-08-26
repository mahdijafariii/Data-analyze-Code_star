using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;
    private readonly IJwtService _jwtService;
    private readonly IRegexService _regexService;


    public UserService(IUserRepository userRepository, ICookieService cookieService,
        IJwtService jwtService, IRegexService regexService)
    {
        _userRepository = userRepository;
        _cookieService = cookieService;
        _jwtService = jwtService;
        _regexService = regexService;
    }

    public async Task<bool> ResetPassword(ClaimsPrincipal userClaim, string password, string confirmPassword)
    {
        var userName = userClaim.FindFirstValue("username");
        var user =await  _userRepository.GetUserByUsername(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _regexService.PasswordCheck(password);
        user.Password = HashPassword(password);
        await _userRepository.UpdateUser(user.Id, user);
        return true;
    }

    public async Task<bool> NewPassword(ClaimsPrincipal userClaim, string oldPassword, string password,
        string confirmPassword)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsername(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Password == HashPassword(oldPassword))
        {
            throw new PasswordMismatchException();
        }

        if (password != confirmPassword)
        {
            throw new PasswordMismatchException();
        }

        _regexService.PasswordCheck(password);
        user.Password = HashPassword(password);
        await _userRepository.UpdateUser(user.Id, user);
        return true;
    }

    public async Task<User> Login(UserLoginModel userLoginModel)
    {
        var user = await _userRepository.GetUserByUsername(userLoginModel.userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Password != HashPassword(userLoginModel.password))
        {
            throw new PasswordMismatchException();
        }

        var token = await _jwtService.GenerateJwtToken(userLoginModel.userName);

        _cookieService.SetCookie("AuthToken", token, userLoginModel.rememberMe);
        return user;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }


    public async Task<bool> UpdateUserInformationByUser(ClaimsPrincipal userClaim, UpdateUserModel updateUserModel)
    {
        var userName = userClaim.FindFirstValue("username");
        var user =  await _userRepository.GetUserByUsername(userName);
        var checkEmail =  await _userRepository.GetUserByEmail(updateUserModel.Email);

        if (checkEmail != null && user.Email != updateUserModel.Email)
            throw new DuplicateUserException();

        _regexService.EmailCheck(updateUserModel.Email);
        _regexService.PhoneNumberCheck(updateUserModel.PhoneNumber);
        await ReplaceUserDetailsAsync(user, updateUserModel);
        return true;
    }

    private async Task ReplaceUserDetailsAsync(User user, UpdateUserModel updateUserModel)
    {
        user.FirstName = updateUserModel.FirstName;
        user.LastName = updateUserModel.LastName;
        user.Email = updateUserModel.Email;
        user.PhoneNumber = updateUserModel.PhoneNumber;
        await _userRepository.UpdateUser(user.Id, user);
    }

    public async Task<bool> UploadImage(ClaimsPrincipal claimsPrincipal, string imageUrl)
    {
        var userName = claimsPrincipal.FindFirstValue("username");
        var user =  await _userRepository.GetUserByUsername(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        user.ImageURL = imageUrl;
        await _userRepository.UpdateUser(user.Id, user);
        return true;
    }
    
}

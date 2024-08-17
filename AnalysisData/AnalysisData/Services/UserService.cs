using System.Data;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.ResetPasswordModel;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;
    private readonly IJwtService _jwtService;
    private readonly IRoleRepository _roleRepository;
    private readonly IRegexService _regexService;

    public UserService(IUserRepository userRepository, ICookieService cookieService, IJwtService jwtService,
        IRoleRepository roleRepository, IRegexService regexService)
    {
        _userRepository = userRepository;
        _cookieService = cookieService;
        _jwtService = jwtService;
        _roleRepository = roleRepository;
        _regexService = regexService;
    }

    public async Task<bool> ResetPassword(ClaimsPrincipal userClaim, string password, string confirmPassword)
    {
        var userName = userClaim.FindFirstValue("username");
        var user = await _userRepository.GetUser(userName);
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
        await _userRepository.UpdateUser(user);
        return true;
    }

    public async Task<User> Login(UserLoginModel userLoginModel)
    {
        var user = await _userRepository.GetUser(userLoginModel.userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        if (user.Password != HashPassword(userLoginModel.password))
        {
            throw new InvalidPasswordException();
        }

        var token = await _jwtService.GenerateJwtToken(userLoginModel.userName);

        _cookieService.SetCookie("AuthToken", token, userLoginModel.rememberMe);
        return user;
    }

    public async Task<bool> Register(UserRegisterModel userRegisterModel)
    {
        var role = await GetRole(userRegisterModel);
        var allUsers = await _userRepository.GetAllUser();
        var existingUser = allUsers.FirstOrDefault(u =>
            u.Username == userRegisterModel.Username || u.Email == userRegisterModel.Email);
        if (existingUser != null)
            throw new DuplicateUserException();
        _regexService.EmailCheck(userRegisterModel.Email);
        _regexService.PasswordCheck(userRegisterModel.Password);
        if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
        {
            throw new PasswordMismatchException();
        }
        _regexService.PhoneNumberCheck(userRegisterModel.PhoneNumber);
        if (userRegisterModel.Password != userRegisterModel.ConfirmPassword)
            throw new PasswordMismatchException();


        if (role == null) return true;
        var user = MakeUser(userRegisterModel, role);
        _userRepository.AddUser(user);

        return true;
    }

    private User MakeUser(UserRegisterModel userRegisterModel, Role role)
    {
        var user = new User
        {
            Username = userRegisterModel.Username,
            Password = HashPassword(userRegisterModel.Password),
            FirstName = userRegisterModel.FirstName,
            LastName = userRegisterModel.LastName,
            Email = userRegisterModel.Email,
            PhoneNumber = userRegisterModel.PhoneNumber,
            Role = role,
            ImageURL = null
        };
        return user;
    }

    private async Task<Role?> GetRole(UserRegisterModel userRegisterModel)
    {
        var role = await _roleRepository.GetRoleByName(userRegisterModel.RoleName.ToLower());
        if (role is null)
        {
            throw new RoleNotFoundException();
        }

        return role;
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
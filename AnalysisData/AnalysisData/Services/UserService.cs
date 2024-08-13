using AnalysisData.CookieSevice.abstractions;
using AnalysisData.Data;
using AnalysisData.JwtService;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.UserManage.LoginModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ICookieService _cookieService;
    private readonly IJwtService _jwtService;

    public UserService(IUserRepository userRepository, ICookieService cookieService,IJwtService jwtService)
    {
        _userRepository = userRepository;
        _cookieService = cookieService;
        _jwtService = jwtService;
    }
    public async Task<List<string>> Login(UserLoginModel userLoginModel)
    {
        if (userLoginModel.userName.Length > 255)
        {
            return null;
        }
        var user = await _userRepository.GetUser(userLoginModel.userName);
        if (user == null || user.Password != userLoginModel.password)
        {
            return null; 
        }
        var token = await _jwtService.GenerateJwtToken(userLoginModel.userName);

        _cookieService.SetCookie("AuthToken", token, userLoginModel.rememberMe);

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

        return roles;
    }
    
}
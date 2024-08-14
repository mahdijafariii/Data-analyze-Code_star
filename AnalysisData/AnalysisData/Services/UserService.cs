using AnalysisData.CookieSevice.abstractions;
using AnalysisData.Data;
using AnalysisData.Exception;
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
        var user = await _userRepository.GetUser(userLoginModel.userName);
        if (user == null )
        {
            throw new NotFoundUserException(); 
        }

        if (user.Password != userLoginModel.password)
        {
            throw new InvalidPasswordException();
        }
        var token = await _jwtService.GenerateJwtToken(userLoginModel.userName);

        _cookieService.SetCookie("AuthToken", token, userLoginModel.rememberMe);

        var roles = user.UserRoles.Select(ur => ur.Role.RoleName).ToList();

        return roles;
    }
    
}
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AnalysisData.CookieService.abstractions;
using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.Services.Business.Abstraction;
using AnalysisData.Services.SecurityPasswordService.Abstraction;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services;

public class UserService : IUserService
{
    private readonly IUserManager _userManager;
    private readonly IPasswordManager _passwordManager;
    private readonly ILoginManager _loginManager;

    public UserService(IUserManager userManager, IPasswordManager passwordManager, ILoginManager loginManager)
    {
        _userManager = userManager;
        _passwordManager = passwordManager;
        _loginManager = loginManager;
    }

    public async Task<bool> ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserAsync(userClaim);
        return await _passwordManager.ResetPasswordAsync(user, password, confirmPassword);
    }

    public async Task<bool> NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserAsync(userClaim);
        return await _passwordManager.NewPasswordAsync(user, oldPassword, password, confirmPassword);
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        return await _loginManager.LoginAsync(userLoginDto);
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal userClaim)
    {
        return await _userManager.GetUserAsync(userClaim);
    }

    public async Task<bool> UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.GetUserAsync(userClaim);
        return await _userManager.UpdateUserInformationAsync(user, updateUserDto);
    }

    public async Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, string imageUrl)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        return await _userManager.UploadImageAsync(user, imageUrl);
    }
}
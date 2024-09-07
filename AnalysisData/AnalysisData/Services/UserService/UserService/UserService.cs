using System.Security.Claims;
using AnalysisData.Dtos.UserDto.UserDto;
using AnalysisData.Models.UserModel;
using AnalysisData.Services.UserService.UserService.Abstraction;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;

namespace AnalysisData.Services.UserService.UserService;

public class UserService : IUserService
{
    private readonly IUserManager _userManager;
    private readonly IPasswordService _passwordService;
    private readonly ILoginManager _loginManager;



    public UserService(IUserManager userManager, IPasswordService passwordService, ILoginManager loginManager)
    {
        _userManager = userManager;
        _passwordService = passwordService;
        _loginManager = loginManager;
    }
    public async Task ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword,string resetPasswordToken)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _passwordService.ResetPasswordAsync(user, password, confirmPassword,resetPasswordToken);
    }

    public async Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _passwordService.NewPasswordAsync(user, oldPassword, password, confirmPassword);
    }

    public async Task<User> LoginAsync(UserLoginDto userLoginDto)
    {
        return await _loginManager.LoginAsync(userLoginDto);
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal userClaim)
    {
        return await _userManager.GetUserFromUserClaimsAsync(userClaim);
    }

    public async Task UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.GetUserFromUserClaimsAsync(userClaim);
        await _userManager.UpdateUserInformationAsync(user, updateUserDto);
    }
    
}
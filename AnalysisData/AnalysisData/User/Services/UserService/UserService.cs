using System.Security.Claims;
using AnalysisData.User.Services.UserService.Abstraction;
using AnalysisData.User.Services.UserService.Business.Abstraction;
using AnalysisData.User.UserDto.UserDto;

namespace AnalysisData.User.Services.UserService;

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

    public async Task ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserAsync(userClaim);
        await _passwordManager.ResetPasswordAsync(user, password, confirmPassword);
    }

    public async Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword)
    {
        var user = await _userManager.GetUserAsync(userClaim);
        await _passwordManager.NewPasswordAsync(user, oldPassword, password, confirmPassword);
    }

    public async Task<Model.User> LoginAsync(UserLoginDto userLoginDto)
    {
        return await _loginManager.LoginAsync(userLoginDto);
    }

    public async Task<Model.User> GetUserAsync(ClaimsPrincipal userClaim)
    {
        return await _userManager.GetUserAsync(userClaim);
    }

    public async Task UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto)
    {
        var user = await _userManager.GetUserAsync(userClaim);
        await _userManager.UpdateUserInformationAsync(user, updateUserDto);
    }

    public async Task UploadImageAsync(ClaimsPrincipal claimsPrincipal, string imageUrl)
    {
        var user = await _userManager.GetUserAsync(claimsPrincipal);
        await _userManager.UploadImageAsync(user, imageUrl);
    }
}
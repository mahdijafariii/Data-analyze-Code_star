using System.Security.Claims;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IUserService
{
    Task<User> LoginAsync(UserLoginDto userLoginDto);
    Task<User> GetUserAsync(ClaimsPrincipal userClaim);
    Task ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserDto updateUserDto);
    Task NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
    Task UploadImageAsync(ClaimsPrincipal claimsPrincipal, string imageUrl);
}
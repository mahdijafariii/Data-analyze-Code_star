using System.Security.Claims;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IUserService
{
    Task<User> LoginAsync(UserLoginModel userLoginModel);
    Task<User> GetUserAsync(ClaimsPrincipal userClaim);
    Task<bool> ResetPasswordAsync(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task<bool> UpdateUserInformationAsync(ClaimsPrincipal userClaim, UpdateUserModel updateUserModel);
    Task<bool> NewPasswordAsync(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
    Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, string imageUrl);
}
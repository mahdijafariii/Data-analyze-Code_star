using System.Security.Claims;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IUserService
{
    Task<User> Login(UserLoginModel userLoginModel);
    Task<bool> ResetPassword(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task<bool> UpdateUserInformationByUser(ClaimsPrincipal userClaim, UpdateUserModel updateUserModel);
    Task<bool> NewPassword(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
    Task<bool> UploadImage(ClaimsPrincipal claimsPrincipal, string imageUrl);
    Task<UserPaginationModel> GetUserById(Guid id);
}
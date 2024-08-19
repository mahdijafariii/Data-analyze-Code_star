using System.Security.Claims;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services.Abstraction;

public interface IUserService
{
    Task<User> Login(UserLoginModel userLoginModel);
    Task<bool> ResetPassword(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task<bool> UpdateUserInformationByUser(Guid id, UpdateUserModel updateUserModel);
    Task<bool> UploadImage(Guid id, string imageUrl);
}
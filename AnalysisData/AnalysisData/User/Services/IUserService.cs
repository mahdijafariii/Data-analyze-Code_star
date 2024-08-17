using System.Security.Claims;
using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

public interface IUserService
{
    Task<User> Login(UserLoginModel userLoginModel);
    Task<bool> Register(UserRegisterModel userRegisterModel);
    Task<bool> ResetPassword(ClaimsPrincipal userClaim, string password, string confirmPassword);
    Task<bool> NewPassword(ClaimsPrincipal userClaim, string oldPassword, string password, string confirmPassword);
}
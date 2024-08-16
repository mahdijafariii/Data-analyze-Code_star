using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

public interface IUserService
{
    Task<User> Login(UserLoginModel userLoginModel);
    Task<bool> Register(UserRegisterModel userRegisterModel);
}
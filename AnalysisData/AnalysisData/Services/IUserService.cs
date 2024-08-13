using AnalysisData.UserManage.LoginModel;

namespace AnalysisData.Services;

public interface IUserService
{
    Task<List<string>> Login(UserLoginModel userLoginModel);
}
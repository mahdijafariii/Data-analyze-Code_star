using AnalysisData.UserManage.LoginModel;
using AnalysisData.UserManage.Model;

namespace AnalysisData.Services.Business.Abstraction;

public interface ILoginManager
{
    Task<User> LoginAsync(UserLoginDto userLoginDto);
}
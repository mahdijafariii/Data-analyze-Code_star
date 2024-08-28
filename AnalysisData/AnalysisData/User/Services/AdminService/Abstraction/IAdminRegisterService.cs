using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

public interface IAdminRegisterService
{
    Task RegisterByAdminAsync(UserRegisterModel userRegisterModel);
    Task AddFirstAdminAsync();
}
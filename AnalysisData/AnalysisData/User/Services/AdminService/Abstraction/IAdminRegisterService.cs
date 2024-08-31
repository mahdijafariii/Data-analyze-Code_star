using AnalysisData.UserManage.RegisterModel;

namespace AnalysisData.Services;

public interface IAdminRegisterService
{
    Task RegisterByAdminAsync(UserRegisterDto userRegisterDto);
    Task AddFirstAdminAsync();
}
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.UpdateModel;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task<bool> Register(UserRegisterModel userRegisterModel);
    Task<bool> UpdateUserInformationByAdmin(Guid id, UpdateAdminModel updateAdminModel);
    Task<bool> DeleteUser(Guid id);
    Task<List<User>> GetUserPagination(int limit, int page);
    
    Task AddFirstAdmin();
    Task<int> GetUserCount();



}
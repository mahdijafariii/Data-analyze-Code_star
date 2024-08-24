using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task Register(UserRegisterModel userRegisterModel);
    Task UpdateUserInformationByAdmin(Guid id, UpdateAdminModel updateAdminModel);
    Task<bool> DeleteUser(Guid id);
    Task<List<UserPaginationModel>> GetUserPagination(int limit, int page);
    
    Task AddFirstAdmin();
    Task<int> GetUserCount();
    Task AddRole(string roleName, string rolePolicy);
    Task DeleteRole(string roleName);



}
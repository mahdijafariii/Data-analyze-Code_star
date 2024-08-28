using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.RolePaginationModel;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task RegisterByAdmin(UserRegisterModel userRegisterModel);
    Task UpdateUserInformationByAdmin(Guid id, UpdateAdminModel updateAdminModel);
    Task<bool> DeleteUser(Guid id);
    Task<List<UserPaginationModel>> GetUserPagination(int limit, int page);
    
    Task AddFirstAdmin();
    Task<int> GetUserCount();
}
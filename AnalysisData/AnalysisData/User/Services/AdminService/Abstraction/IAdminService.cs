using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.RolePaginationModel;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task UpdateUserInformationByAdminAsync(Guid id, UpdateAdminModel updateAdminModel);
    Task<bool> DeleteUserAsync(Guid id);
    Task<List<UserPaginationModel>> GetAllUserAsync(int limit, int page);
    Task<int> GetUserCountAsync();
}
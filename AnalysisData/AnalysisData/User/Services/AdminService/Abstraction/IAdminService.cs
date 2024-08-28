using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RegisterModel;
using AnalysisData.UserManage.RolePaginationModel;
using AnalysisData.UserManage.UpdateModel;
using AnalysisData.UserManage.UserPaginationModel;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task UpdateUserInformationByAdminAsync(Guid id, UpdateAdminDto updateAdminDto);
    Task<bool> DeleteUserAsync(Guid id);
    Task<List<UserPaginationDto>> GetAllUserAsync(int limit, int page);
    Task<int> GetUserCountAsync();
}
using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<Role> GetRoleByIdAsync(int roleId);
    Task<Role> GetRoleByNameAsync(string roleName);
    Task<bool> AddRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(string roleId);
    Task<List<Role>> GetAllRolesPaginationAsync(int page, int limit);
    Task<int> GetRolesCountAsync();
}
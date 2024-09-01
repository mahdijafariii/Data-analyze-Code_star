using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<Role> GetRoleByIdAsync(int roleId);
    Task<Role> GetRoleByNameAsync(string roleName);
    Task AddRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(string roleId);
    Task<List<Role>> GetAllRolesPaginationAsync(int page, int limit);
    Task<int> GetRolesCountAsync();
    Task<IEnumerable<string>> GetRolesByPolicyAsync(string policy);
}
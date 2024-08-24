using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<Role> GetRoleById(int roleId);
    Task<Role> GetRoleByName(string roleName);
    Task<bool> AddRole(Role role);
    Task<bool> DeleteRole(string roleId);
}
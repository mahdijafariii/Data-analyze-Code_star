
using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<Role> GetRoleById(int roleId);
    Task<Role> GetRoleByName(string roleName);
    Task<IReadOnlyList<Role>> GetAllRole();
    
    bool AddRole(Role role);
    bool DeleteRole(string roleName);
    void UpdateRole(Role role);
}
using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<Role> GetRoleByID(int roleId);
    Task<Role> GetRoleByName(string roleName);
    bool AddRole(Role role);
    bool DeleteRole(int roleId);
}
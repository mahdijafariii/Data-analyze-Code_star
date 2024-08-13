using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.RoleRepository.Abstraction;

public interface IRoleRepository
{
    Task<Role> GetRole(int rolId);
    bool AddRole(Role role);
    bool DeleteRole(int roleId);
}
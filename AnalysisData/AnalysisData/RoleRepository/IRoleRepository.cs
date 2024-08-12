using AnalysisData.UserManage.Model;

namespace AnalysisData.RoleRepository;

public interface IRoleRepository
{
    Task<Role> GetRoles(int rolId);
}
using AnalysisData.UserManage.Model;

namespace AnalysisData.Services.Abstraction;

public interface IAdminService
{
    Task<IReadOnlyList<User>> GetAllUsers();
    Task<IReadOnlyList<Role>> GetAllRoles();
    // User UpdateUserInformation(string username);
    // Role UpdateRoleInformation(string roleName);
    bool DeleteUser(string username);
    bool DeleteRole(string roleName);
    bool AddRole(string roleName);
}
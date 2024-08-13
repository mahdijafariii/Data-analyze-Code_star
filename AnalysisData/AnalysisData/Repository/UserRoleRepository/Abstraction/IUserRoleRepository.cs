using AnalysisData.UserManage.Model;

namespace AnalysisData.Repository.UserRoleRepository.Abstraction;

public interface IUserRoleRepository
{
    void Add(UserRole userRole);
    bool DeleteUserInUserRole(int userId);
    bool DeleteRoleInUserRole(int roleId);
}
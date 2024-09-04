using AnalysisData.User.UserDto.RoleDto;

namespace AnalysisData.User.Services.RoleService.Abstraction;

public interface IRoleManagementService
{
    Task<int> GetRoleCount();
    Task AddRole(string roleName, string rolePolicy);
    Task DeleteRole(string roleName);
    Task<List<RolePaginationDto>> GetRolePagination(int page, int limit);
}
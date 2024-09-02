using AnalysisData.UserDto.RoleDto;

namespace AnalysisData.Services.RoleService.Abstraction;

public interface IRoleManagementService
{
    Task<int> GetRoleCount();
    Task AddRole(string roleName, string rolePolicy);
    Task DeleteRole(string roleName);
    Task<List<RolePaginationDto>> GetRolePagination(int page, int limit);
}
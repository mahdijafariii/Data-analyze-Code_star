using AnalysisData.Exception.UserException;
using AnalysisData.User.Model;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using AnalysisData.User.Services.RoleService.Abstraction;
using AnalysisData.User.UserDto.RoleDto;

namespace AnalysisData.User.Services.RoleService;

public class RoleManagementService : IRoleManagementService
{
    private readonly IRoleRepository _roleRepository;

    public RoleManagementService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<int> GetRoleCount()
    {
        return await _roleRepository.GetRolesCountAsync();
    }

    public async Task<List<RolePaginationDto>> GetRolePagination(int page, int limit)
    {
        var users = await _roleRepository.GetAllRolesPaginationAsync(page, limit);
        var paginationRoles = users.Select(x => new RolePaginationDto()
        {
            Id = x.Id.ToString(), Name = x.RoleName, Policy = x.RolePolicy
        });
        return paginationRoles.ToList();
    }


    public async Task DeleteRole(string roleName)
    {
        var roleExist = await _roleRepository.GetRoleByNameAsync(roleName);
        if (roleExist == null)
        {
            throw new RoleNotFoundException();
        }

        await _roleRepository.DeleteRoleAsync(roleName);
    }

    public async Task AddRole(string roleName, string rolePolicy)
    {
        var roleExist = await _roleRepository.GetRoleByNameAsync(roleName.ToLower());
        if (roleExist != null)
        {
            throw new DuplicateRoleExistException();
        }
        var role = new Role { RoleName = roleName.ToLower(), RolePolicy = rolePolicy.ToLower() };
        await _roleRepository.AddRoleAsync(role);
    }
}
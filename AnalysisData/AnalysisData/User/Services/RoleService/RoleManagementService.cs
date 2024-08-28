using AnalysisData.Exception;
using AnalysisData.JwtService.abstractions;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;
using AnalysisData.UserManage.RolePaginationModel;

namespace AnalysisData.Services;

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

    public async Task<List<RolePaginationModel>> GetRolePagination(int page, int limit)
    {
        var users = await _roleRepository.GetAllRolesPaginationAsync(page, limit);
        var paginationRoles = users.Select(x => new RolePaginationModel()
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
        var roleExist = await _roleRepository.GetRoleByNameAsync(roleName);
        if (roleExist != null)
        {
            throw new DuplicateRoleExistException();
        }

        var role = new Role { RoleName = roleName, RolePolicy = rolePolicy };
        await _roleRepository.AddRoleAsync(role);
    }
}
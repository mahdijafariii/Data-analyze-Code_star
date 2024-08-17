using System.Xml.Schema;
using AnalysisData.Repository.RoleRepository;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Repository.UserRepository;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.Abstraction;
using AnalysisData.UserManage.Model;

namespace AnalysisData.Services;

public class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;


    public AdminService(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public Task<IReadOnlyList<User>> GetAllUsers()
    {
        return _userRepository.GetAllUser(); // maybe null??
    }

    public Task<IReadOnlyList<Role>> GetAllRoles()
    {
        return _roleRepository.GetAllRole(); // maybe null??
    }

    // public User UpdateUserInformation(string username)
    // {
    //     
    // }
    //
    // public Role UpdateRoleInformation(string roleName)
    // {
    // }

    public bool DeleteUser(string username)
    {
        if (_userRepository.DeleteUser(username))
        {
            return true;
        }

        return false; //exception
    }

    public bool DeleteRole(string roleName)
    {
        if (_roleRepository.DeleteRole(roleName))
            return true;
        return false; //exception
    }

    public bool AddRole(string roleName)
    {
        var role = new Role { RoleName = roleName };
        var check = _roleRepository.AddRole(role);
        if (check) return true;
        return false;
    }
}
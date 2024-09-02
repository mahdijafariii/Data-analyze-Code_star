using System.Reflection;
using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.Repository.RoleRepository.Abstraction;
using AnalysisData.Services.PermissionService.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Services.PermissionService;

public class PermissionService : IPermissionService
{
    private readonly IRoleRepository _roleRepository;

    public PermissionService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    private Dictionary<string, List<string>> GetRolePermissions(Assembly assembly, string policyName)
    {
        var rolePermissions = new Dictionary<string, List<string>>();

        var controllers = assembly.GetTypes()
            .Where(type => typeof(ControllerBase).IsAssignableFrom(type) && !type.IsAbstract);

        foreach (var controller in controllers)
        {
            var actions = controller.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                .Where(m => !m.IsDefined(typeof(NonActionAttribute)));

            foreach (var action in actions)
            {
                var authorizeAttributes = controller.GetCustomAttributes<AuthorizeAttribute>(true)
                    .Concat(action.GetCustomAttributes<AuthorizeAttribute>(true));

                foreach (var authorizeAttribute in authorizeAttributes)
                {
                    if (authorizeAttribute.Policy?.Split(',').Contains(policyName) != true) continue;
                    var controllerName = controller.Name.Replace("Controller", "");
                    var actionName = action.Name;

                    if (!rolePermissions.ContainsKey(controllerName))
                    {
                        rolePermissions[controllerName] = new List<string>();
                    }

                    rolePermissions[controllerName].Add(actionName);
                }
            }
        }

        return rolePermissions;
    }


    public async Task<IEnumerable<string>> GetPermission(ClaimsPrincipal userClaims)
    {
        var roleName = userClaims.FindFirstValue(ClaimTypes.Role);
        if (roleName is null)
        {
            throw new UserNotFoundException();
        }
        var existRole = await _roleRepository.GetRoleByNameAsync(roleName);
        if (existRole is null)
        {
            throw new RoleNotFoundException();
        }
        var rolePermissions = GetRolePermissions(Assembly.GetExecutingAssembly(), existRole.RolePolicy);
        var permissions = rolePermissions.Values.SelectMany(x => x);
        return permissions;
    }
}
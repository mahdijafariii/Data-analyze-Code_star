using System.Reflection;
using System.Security.Claims;
using AnalysisData.Exception.UserException;
using AnalysisData.User.Repository.RoleRepository.Abstraction;
using AnalysisData.User.Services.PermissionService.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.User.Services.PermissionService;

public class PermissionService : IPermissionService
{
    private readonly IRoleRepository _roleRepository;

    public PermissionService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    private Dictionary<string, List<string>> GetRolePermissions(Assembly assembly)
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
                    var policies = authorizeAttribute.Policy?.Split(',') ?? new string[0];

                    foreach (var policy in policies)
                    {
                        if (!rolePermissions.ContainsKey(policy))
                        {
                            rolePermissions[policy] = new List<string>();
                        }

                        var controllerName = controller.Name.Replace("Controller", "");
                        var actionName = action.Name;

                        rolePermissions[policy].Add($"{actionName}");
                    }
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
        var rolePermissions = GetRolePermissions(Assembly.GetExecutingAssembly());
        IEnumerable<string> permissions;
        
        var allowedPolicies = GetAllowedPolicies(existRole.RolePolicy);
        permissions = FilterByPolicy(rolePermissions, allowedPolicies);

        return permissions;
        
    }
    
    private string[] GetAllowedPolicies(string rolePolicy)
    {
        switch (rolePolicy.ToLower())
        {
            case "gold":
                return new[] { "gold", "silver", "bronze" };
            case "silver":
                return new[] { "silver", "bronze" };
            case "bronze":
                return new[] { "bronze" };
            default:
                return Array.Empty<string>();
        }
    }
    private IEnumerable<string> FilterByPolicy(Dictionary<string, List<string>> rolePermissions, string[] allowedPolicies)
    {
        var filteredPermissions = new List<string>();

        // Iterate over each allowed policy
        foreach (var policy in allowedPolicies)
        {
            // Check if the policy exists in the dictionary
            if (rolePermissions.TryGetValue(policy, out var actions))
            {
                // Add the actions to the result
                filteredPermissions.AddRange(actions);
            }
        }

        return filteredPermissions;
    }
}
using System.Reflection;
using System.Security.Claims;
using AnalysisData.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.Services;

public class PermissionService : IPermissionService
{
    private Dictionary<string, List<string>> GetRolePermissions(Assembly assembly, string roleName)
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
                    if (authorizeAttribute.Roles?.Split(',').Contains(roleName) != true) continue;
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


    public IEnumerable<string> GetPermission(ClaimsPrincipal userClaims)
    {
        var role = userClaims.FindFirstValue(ClaimTypes.Role);
        var rolePermissions = GetRolePermissions(Assembly.GetExecutingAssembly(), role);
        var permissions = rolePermissions.Values.SelectMany(x => x);
        return permissions;
    }
}
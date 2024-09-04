using System.Security.Claims;

namespace AnalysisData.User.Services.PermissionService.Abstraction;

public interface IPermissionService
{
    Task<IEnumerable<string>> GetPermission(ClaimsPrincipal userClaims);

}
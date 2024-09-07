using System.Security.Claims;

namespace AnalysisData.Services.PermissionService.Abstraction;

public interface IPermissionService
{
    Task<IEnumerable<string>> GetPermission(ClaimsPrincipal userClaims);

}
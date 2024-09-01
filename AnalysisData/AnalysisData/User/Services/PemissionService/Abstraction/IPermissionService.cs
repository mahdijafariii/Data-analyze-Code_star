using System.Security.Claims;

namespace AnalysisData.Services.Abstraction;

public interface IPermissionService
{
    Task<IEnumerable<string>> GetPermission(ClaimsPrincipal userClaims);

}
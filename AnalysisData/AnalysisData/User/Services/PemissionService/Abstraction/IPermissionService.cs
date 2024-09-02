using System.Security.Claims;

namespace AnalysisData.Services.PemissionService.Abstraction;

public interface IPermissionService
{
    IEnumerable<string> GetPermission(ClaimsPrincipal userClaims);
}
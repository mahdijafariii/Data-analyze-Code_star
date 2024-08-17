using System.Security.Claims;

namespace AnalysisData.Services;

public interface IPermissionService
{
    string GetPermission(ClaimsPrincipal userClaims);
}
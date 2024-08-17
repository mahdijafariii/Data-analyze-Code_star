using System.Security.Claims;

namespace AnalysisData.Services.Abstraction;

public interface IPermissionService
{
    string GetPermission(ClaimsPrincipal userClaims);
}
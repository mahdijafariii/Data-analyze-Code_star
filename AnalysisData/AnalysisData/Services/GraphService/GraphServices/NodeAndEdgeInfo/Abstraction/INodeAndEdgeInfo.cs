using System.Security.Claims;

namespace AnalysisData.Services.GraphService.GraphServices.NodeAndEdgeInfo.Abstraction;

public interface INodeAndEdgeInfo
{
    Task<Dictionary<string, string>> GetNodeInformationAsync(ClaimsPrincipal claimsPrincipal, Guid nodeId);
    Task<Dictionary<string, string>> GetEdgeInformationAsync(ClaimsPrincipal claimsPrincipal, Guid edgeId);
}
using System.Security.Claims;

namespace AnalysisData.EAV.Service.GraphServices.NodeAndEdgeServices;

public interface INodeAndEdgeInfo
{
    Task<Dictionary<string, string>> GetNodeInformationAsync(ClaimsPrincipal claimsPrincipal, string headerUniqueId);
    Task<Dictionary<string, string>> GetEdgeInformationAsync(ClaimsPrincipal claimsPrincipal, int edgeId);
}
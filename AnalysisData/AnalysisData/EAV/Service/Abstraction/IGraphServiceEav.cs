using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Service.Abstraction;

public interface IGraphServiceEav
{
    Task<Dictionary<string, string>> GetNodeInformation(ClaimsPrincipal claimsPrincipal, string headerUniqueId);
    Task<Dictionary<string, string>> GetEdgeInformation(ClaimsPrincipal claimsPrincipal, int edgeId);

    Task<PaginatedNodeListDto> GetAllNodesAsync(ClaimsPrincipal claimsPrincipal, int pageIndex, int pageSize,
        int? categoryId = null);

    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(ClaimsPrincipal claimsPrincipal,
        string id);

    Task<IEnumerable<EntityNode>> SearchInEntityNodeName(ClaimsPrincipal claimsPrincipal, string inputSearch,
        string type);
}
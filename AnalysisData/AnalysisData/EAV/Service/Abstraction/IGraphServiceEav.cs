using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Service.Abstraction;

public interface IGraphServiceEav
{
    Task<Dictionary<string, string>> GetNodeInformation(ClaimsPrincipal claimsPrincipal, string headerUniqueId);
    Task<Dictionary<string, string>> GetEdgeInformation(int edgeId);

    Task<PaginatedListDto> GetNodesPaginationAsync(ClaimsPrincipal claimsPrincipal, int pageIndex, int pageSize,
        int? categoryId = null);
    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(string id);
    Task<IEnumerable<EntityNode>> SearchEntityNodeName(string inputSearch, string searchType);
}
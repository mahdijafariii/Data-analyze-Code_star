using System.Security.Claims;
using AnalysisData.Graph.Dto.NodeDto;

namespace AnalysisData.Graph.Service.GraphServices.AllNodesData.Abstraction;

public interface INodePaginationService
{
    Task<PaginatedNodeListDto> GetAllNodesAsync(ClaimsPrincipal claimsPrincipal, int pageIndex, int pageSize,
        int? categoryId = null);
}
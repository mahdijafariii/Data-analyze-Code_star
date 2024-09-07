using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.NodeDto;

namespace AnalysisData.Services.GraphService.GraphServices.AllNodesData.Abstraction;

public interface INodePaginationService
{
    Task<PaginatedNodeListDto> GetAllNodesAsync(ClaimsPrincipal claimsPrincipal, int pageIndex, int pageSize,
        int? categoryId = null);
}
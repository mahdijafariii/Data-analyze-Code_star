using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Service.Abstraction;

public interface INodePaginationService
{
    Task<PaginatedNodeListDto> GetAllNodesAsync(ClaimsPrincipal claimsPrincipal, int pageIndex, int pageSize,
        int? categoryId = null);
    
}
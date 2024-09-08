using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Services.GraphService.GraphServices.Search.Abstraction;

public interface IGraphSearchService
{
    Task<PaginationSearchDto> SearchInEntityNodeNameAsync(ClaimsPrincipal claimsPrincipal, string inputSearch,
        string type,int pageIndex,  int pageSize );
}
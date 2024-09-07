using System.Security.Claims;
using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Services.GraphService.GraphServices.Search.Abstraction;

public interface IGraphSearchService
{
    Task<IEnumerable<EntityNode>> SearchInEntityNodeNameAsync(ClaimsPrincipal claimsPrincipal, string inputSearch,
        string type);
}
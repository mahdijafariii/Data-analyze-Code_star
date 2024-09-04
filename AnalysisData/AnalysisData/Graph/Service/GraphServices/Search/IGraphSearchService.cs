using System.Security.Claims;
using AnalysisData.Graph.Model.Node;

namespace AnalysisData.Graph.Service.GraphServices.Search;

public interface IGraphSearchService
{
    Task<IEnumerable<EntityNode>> SearchInEntityNodeNameAsync(ClaimsPrincipal claimsPrincipal, string inputSearch,
        string type);
}
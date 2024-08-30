using System.Security.Claims;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Service.GraphSevices;

public interface IGraphSearchService
{
    Task<IEnumerable<EntityNode>> SearchInEntityNodeNameAsync(ClaimsPrincipal claimsPrincipal, string inputSearch,
        string type);
}
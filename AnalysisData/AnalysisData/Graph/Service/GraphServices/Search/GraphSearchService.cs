using System.Security.Claims;
using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.GraphNodeRepository;
using AnalysisData.Graph.Service.GraphServices.Search.Abstraction;

namespace AnalysisData.Graph.Service.GraphServices.Search;

public class GraphSearchService : IGraphSearchService
{
    private readonly IGraphNodeRepository _graphNodeRepository;

    public GraphSearchService(IGraphNodeRepository graphNodeRepository)
    {
        _graphNodeRepository = graphNodeRepository;
    }

    public async Task<IEnumerable<EntityNode>> SearchInEntityNodeNameAsync(ClaimsPrincipal claimsPrincipal,
        string inputSearch, string type)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<EntityNode> entityNodes;
        if (role != "dataanalyst")
        {
            entityNodes = await SearchEntityInNodeNameForAdminAsync(inputSearch, type);
        }
        else
        {
            entityNodes = await SearchEntityInNodeNameForUserAsync(username, inputSearch, type);
        }

        if (!entityNodes.Any())
        {
            throw new NodeNotFoundException();
        }

        return entityNodes;
    }

    private async Task<IEnumerable<EntityNode>> SearchEntityInNodeNameForAdminAsync(string inputSearch, string type)
    {
        IEnumerable<EntityNode> entityNodes;
        var searchType = type.ToLower();
        switch (searchType)
        {
            case "startswith":
                entityNodes = await _graphNodeRepository.GetNodeStartsWithSearchInputForAdminAsync(inputSearch);
                break;
            case "endswith":
                entityNodes = await _graphNodeRepository.GetNodeEndsWithSearchInputForAdminAsync(inputSearch);
                break;
            default:
                entityNodes = await _graphNodeRepository.GetNodeContainSearchInputForAdminAsync(inputSearch);
                break;
        }

        return entityNodes;
    }

    private async Task<IEnumerable<EntityNode>> SearchEntityInNodeNameForUserAsync(string username, string inputSearch,
        string type)
    {
        IEnumerable<EntityNode> entityNodes;
        var searchType = type.ToLower();
        var usernameGuid = Guid.Parse(username);
        switch (searchType)
        {
            case "startswith":
                entityNodes =
                    await _graphNodeRepository.GetNodeStartsWithSearchInputForUserAsync(usernameGuid, inputSearch);
                break;
            case "endswith":
                entityNodes = await _graphNodeRepository.GetNodeEndsWithSearchInputForUserAsync(usernameGuid, inputSearch);
                break;
            default:
                entityNodes = await _graphNodeRepository.GetNodeContainSearchInputForUserAsync(usernameGuid, inputSearch);
                break;
        }

        return entityNodes;
    }
}
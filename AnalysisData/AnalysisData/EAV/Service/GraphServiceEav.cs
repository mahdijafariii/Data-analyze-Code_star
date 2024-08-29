using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.Exception;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.EAV.Service;

public class GraphServiceEav : IGraphServiceEav
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IGraphEdgeRepository _graphEdgeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly ICategoryService _categoryService;
    

    
    public GraphServiceEav(IGraphNodeRepository graphNodeRepository, IGraphEdgeRepository graphEdgeRepository, IEntityNodeRepository entityNodeRepository, IEntityEdgeRepository entityEdgeRepository,IAttributeNodeRepository attributeNodeRepository, ICategoryService categoryService)
    {
        _graphNodeRepository = graphNodeRepository;
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
        _categoryService = categoryService;
        _graphEdgeRepository = graphEdgeRepository;
    }

    public async Task<PaginatedListDto> GetNodesPaginationAsync(ClaimsPrincipal claimsPrincipal, int pageIndex, int pageSize, int? categoryId = null)
    {
        var valueNodes = await GetEntityNodesForPaginationAsync(claimsPrincipal, categoryId);

        string categoryName = null;
        if (categoryId.HasValue)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId.Value);
            categoryName = category?.Name;
        }

        var groupedNodes = valueNodes.Select(g => new PaginationNodeDto
            {
                Id = g.Id,
                EntityName = g.Name,
            })
            .ToList();

        var count = groupedNodes.Count;
        var items = groupedNodes
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedListDto(items, pageIndex, count, categoryName);
    }

    private async Task<IEnumerable<EntityNode>> GetEntityNodesForPaginationAsync(ClaimsPrincipal claimsPrincipal,int? category = null)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<EntityNode> valueNodes = new List<EntityNode>();
        if (category == null && role != "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityNodesAsAdminAsync();
        }
        else if (role != "dataanalyst" && category != null)
        {
            valueNodes = await _graphNodeRepository.GetEntityAsAdminNodesWithCategoryIdAsync(category.Value);
        }
        else if (category != null && role == "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityAsUserNodesWithCategoryIdAsync(username,category.Value);
        }
        else if (category == null && role == "dataanalyst")
        {
            valueNodes = await _graphNodeRepository.GetEntityNodesAsUserAsync(username);
        }

        if (!valueNodes.Any() && category != null)
        {
            throw new CategoryResultNotFoundException();
        }

        if (valueNodes is null)
        {
            throw new NodeNotFoundException();
        }

        return valueNodes;
    }
    
    public async Task<Dictionary<string, string>> GetNodeInformation(ClaimsPrincipal claimsPrincipal,string headerUniqueId)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<dynamic> result = Enumerable.Empty<dynamic>();
        if (role != "dataanalyst")
        {
            result = await _graphNodeRepository.GetNodeAttributeValue(headerUniqueId);
        }
        else if (await _graphNodeRepository.IsNodeAccessibleByUser(username, headerUniqueId))
        {
            result = await _graphNodeRepository.GetNodeAttributeValue(headerUniqueId);
        }
        if (result.Count()==0)
        {
            throw new NodeNotFoundException();
        }
        var output = new Dictionary<string, string>();
        foreach (var item in result)
        {
            output[item.Attribute] = item.Value;
        }
        return output;
    }
    public async Task<Dictionary<string, string>> GetEdgeInformation(ClaimsPrincipal claimsPrincipal,int edgeId)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<dynamic> result = Enumerable.Empty<dynamic>();
        if (role != "dataanalyst")
        {
            result = await _graphEdgeRepository.GetEdgeAttributeValues(edgeId);
        }
        else if (await _graphEdgeRepository.IsEdgeAccessibleByUser(username, edgeId))
        {
            result = await _graphEdgeRepository.GetEdgeAttributeValues(edgeId);
        }
        if (result.Count() == 0)
        {
            throw new EdgeNotFoundException();
        }

        var output = new Dictionary<string, string>();

        foreach (var item in result)
        {
            output[item.Attribute] = item.Value;
        }

        return output;
    }


    public async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(ClaimsPrincipal claimsPrincipal,string id)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        var node = await _entityNodeRepository.GetByIdAsync(id);
  
        (IEnumerable<NodeDto> nodes, IEnumerable<EdgeDto> edges) result;

        if (role != "dataanalyst")
        {
            result = await GetNodeRelations(id);
        }
        else if (await _graphNodeRepository.IsNodeAccessibleByUser(username, node.Name))
        {
            result = await GetNodeRelations(id);
        }
        else
        {
            throw new NodeNotFoundException();
        }

        if (!result.nodes.Any() && !result.edges.Any())
        {
            throw new NodeNotFoundException();
        }

        return result;
    }

    private async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetNodeRelations(string id)
    {
        var node = await _entityNodeRepository.GetByIdAsync(id);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        var edges = await _entityEdgeRepository.FindNodeLoopsAsync(node.Id);
        var uniqueNodes = edges.SelectMany(x => new[] { x.EntityIDTarget, x.EntityIDSource }).Distinct().ToList();
        var nodes = await _entityNodeRepository.GetNodesOfEdgeList(uniqueNodes);
        var nodeDto = nodes.Select(x => new NodeDto() { Id = x.Id.ToString(), Label = x.Name });
        var edgeDto = edges.Select(x => new EdgeDto()
            { From = x.EntityIDSource, To = x.EntityIDTarget, Id = x.Id.ToString() });
        return (nodeDto, edgeDto);
    }

    public async Task<IEnumerable<EntityNode>> SearchInEntityNodeName(ClaimsPrincipal claimsPrincipal,string inputSearch, string type)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<EntityNode> entityNodes;
        if (role != "dataanalyst")
        {
            entityNodes = await SearchEntityInNodeNameAsAdmin(inputSearch, type);
        }
        else
        {
            entityNodes = await SearchEntityInNodeNameAsUser(username,inputSearch,type);
        }
        if (!entityNodes.Any())
        {
            throw new NodeNotFoundException();
        }

        return entityNodes;
    }

    private async Task<IEnumerable<EntityNode>> SearchEntityInNodeNameAsAdmin(string inputSearch, string type)
    {
        IEnumerable<EntityNode> entityNodes;
        var searchType = type.ToLower();
        switch (searchType)
        {
            case "startswith":
                entityNodes = await _graphNodeRepository.GetNodeStartsWithSearchInputAsAdmin(inputSearch);
                break;
            case "endswith":
                entityNodes = await _graphNodeRepository.GetNodeEndsWithSearchInputAsAdmin(inputSearch);
                break;
            default:
                entityNodes = await _graphNodeRepository.GetNodeContainSearchInputAsAdmin(inputSearch);
                break;
        }

        return entityNodes;
    }
    
    private async Task<IEnumerable<EntityNode>> SearchEntityInNodeNameAsUser(string username,string inputSearch, string type)
    {
        IEnumerable<EntityNode> entityNodes;
        var searchType = type.ToLower();
        switch (searchType)
        {
            case "startswith":
                entityNodes = await _graphNodeRepository.GetNodeStartsWithSearchInputAsUser(username,inputSearch);
                break;
            case "endswith":
                entityNodes = await _graphNodeRepository.GetNodeEndsWithSearchInputAsUser(username,inputSearch);
                break;
            default:
                entityNodes = await _graphNodeRepository.GetNodeContainSearchInputAsUser(username,inputSearch);
                break;
        }

        return entityNodes;
    }
}
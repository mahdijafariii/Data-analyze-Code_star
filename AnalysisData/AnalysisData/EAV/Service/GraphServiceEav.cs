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

    public async Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize, int? categoryId = null)
    {
        var valueNodes = await GetEntityNodesForPaginationAsync(categoryId);

        string categoryName = null;
        if (categoryId.HasValue)
        {
            var category = await _categoryService.GetCategoryByIdAsync(categoryId.Value);
            categoryName = category?.Name;
        }

        var groupedNodes = valueNodes.Select(g => new PaginationNodeDto
            {
                EntityName = g.Name,
            })
            .ToList();

        var count = groupedNodes.Count;
        var items = groupedNodes
            .Skip(pageIndex * pageSize)
            .Take(pageSize)
            .Select(x => x.EntityName)  // Extract EntityName to match List<string>
            .ToList();

        return new PaginatedListDto(items, pageIndex, count, categoryName);
    }

    private async Task<IEnumerable<EntityNode>> GetEntityNodesForPaginationAsync(int? category = null)
    {
        var valueNodes = category == null
            ? await _graphNodeRepository.GetEntityNodesAsync()
            : await _graphNodeRepository.GetEntityNodesWithCategoryIdAsync(category.Value);

        if (!valueNodes.Any() && category != null)
        {
            throw new CategoryResultNotFoundException();
        }

        return valueNodes;
    }


    public async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(string id)
    {
        var node = await _entityNodeRepository.GetByIdAsync(id);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        var edges = await _entityEdgeRepository.FindNodeLoopsAsync(node.Id);
        var uniqueNodes = edges.SelectMany(x => new[] { x.EntityIDTarget, x.EntityIDSource }).Distinct().ToList();
        var nodes = await _entityNodeRepository.GetNodesOfEdgeList(uniqueNodes);
        var nodeDto = nodes.Select(x => new NodeDto() { ID = x.Id.ToString(), Lable = x.Name });
        var edgeDto = edges.Select(x => new EdgeDto()
            { From = x.EntityIDSource, To = x.EntityIDTarget, Id = x.Id.ToString() });
        return (nodeDto, edgeDto);
    }

    public async Task<Dictionary<string, string>> GetNodeInformation(string headerUniqueId)
    {
        var result = await _graphNodeRepository.GetNodeAttributeValue(headerUniqueId);
        if (result is null)
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

    public async Task<Dictionary<string, string>> GetEdgeInformation(int edgeId)
    {
        var result = await _graphEdgeRepository.GetEdgeAttributeValues(edgeId);
        if (result is null)
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

    public async Task<IEnumerable<EntityNode>> SearchEntityNodeName(string inputSearch, string type)
    {
        IEnumerable<EntityNode> entityNodes;
        var searchType = type.ToLower();
        switch (searchType)
        {
            case "startswith":
                entityNodes = await _graphNodeRepository.GetNodeStartsWithSearchInput(inputSearch);
                break;
            case "endswith":
                entityNodes = await _graphNodeRepository.GetNodeEndsWithSearchInput(inputSearch);
                break;
            default:
                entityNodes = await _graphNodeRepository.GetNodeContainSearchInput(inputSearch);
                break;
        }

        if (!entityNodes.Any())
        {
            throw new NodeNotFoundException();
        }

        return entityNodes;
    }
}
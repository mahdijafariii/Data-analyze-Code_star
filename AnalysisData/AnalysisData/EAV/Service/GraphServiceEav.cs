using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.Exception;

namespace AnalysisData.EAV.Service;

public class GraphServiceEav : IGraphServiceEav
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IGraphEdgeRepository _graphEdgeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly IAttributeNodeRepository _attributeNodeRepository;


    
    public GraphServiceEav(IGraphNodeRepository graphNodeRepository, IGraphEdgeRepository graphEdgeRepository, IEntityNodeRepository entityNodeRepository, IEntityEdgeRepository entityEdgeRepository,IAttributeNodeRepository attributeNodeRepository)
    {
        _graphNodeRepository = graphNodeRepository;
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
        _attributeNodeRepository = attributeNodeRepository;
        _graphEdgeRepository = graphEdgeRepository;
    }

    public async Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize, string category)
    {
        var valueNodes = await GetEntityNodesForPaginationAsync(category);

        var groupedNodes = valueNodes.Select(g => new PaginationNodeDto
            {
                EntityName = g.Name,
            })
            .ToList();

        var count = groupedNodes.Count;
        var items = groupedNodes.Select(x => x.EntityName).Skip((pageIndex) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedListDto(items, pageIndex, count, category);
    }

    private async Task<IEnumerable<EntityNode>> GetEntityNodesForPaginationAsync(string category)
    {
        var lowerCaseCategory = category.ToLower();


        var valueNodes = lowerCaseCategory == "all"
            ?  await _graphNodeRepository.GetEntityNodesAsync()
            : await _graphNodeRepository.GetEntityNodesWithCategoryAsync(lowerCaseCategory);
        if (!valueNodes.Any() && lowerCaseCategory != "all")
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

}
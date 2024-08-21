using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;

namespace AnalysisData.EAV.Service;

public class GraphServiceEav : IGraphServiceEav
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly IAttributeNodeRepository _attributeNodeRepository;


    public GraphServiceEav(IEntityNodeRepository entityNodeRepository, IGraphNodeRepository graphNodeRepository,
        IEntityEdgeRepository entityEdgeRepository, IAttributeNodeRepository attributeNodeRepository)
    {
        _entityNodeRepository = entityNodeRepository;
        _graphNodeRepository = graphNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
        _attributeNodeRepository = attributeNodeRepository;
    }

    public async Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize, string category)
    {
        var valueNodes = GetEntityNodesForPagination(category);
        var typeCategory =await _attributeNodeRepository.GetTypeOfCategory();


        var groupedNodes = valueNodes.Select(g => new PaginationNodeDto
            {
                EntityName = g.Name,
            })
            .ToList();

        var count = groupedNodes.Count;
        var items = groupedNodes.Select(x=> x.EntityName).Skip((pageIndex) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedListDto(items, pageIndex, count, typeCategory);
    }

    private IEnumerable<EntityNode> GetEntityNodesForPagination(string category)
    {
        IEnumerable<EntityNode> valueNodes = new List<EntityNode>();
        if (category == "all")
        {
            valueNodes = _graphNodeRepository.GetEntityNodesAsync();
        }
        else
        {
            valueNodes = _graphNodeRepository.GetEntityNodesWithCategoryAsync(category);
        }

        return valueNodes;
    }


    public async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(string id)
    {
        var node = await _entityNodeRepository.GetEntityByNameAsync(id);
        var edges = await _entityEdgeRepository.FindNodeLoopsAsync(node.Id);
        var uniqueNodes = edges.SelectMany(x => new[] { x.EntityIDTarget, x.EntityIDSource }).Distinct().ToList();
        var nodes = await _entityNodeRepository.GetNodesOfEdgeList(uniqueNodes);
        var nodeDto = nodes.Select(x => new NodeDto() { EntityID = x.Id.ToString(), EntityName = x.Name });
        var edgeDto = edges.Select(x => new EdgeDto()
            { From = x.EntityIDSource, To = x.EntityIDTarget, EdgeId = x.Id.ToString() });
        return (nodeDto, edgeDto);
    }
}
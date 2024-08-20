using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Services;

namespace AnalysisData.EAV.Service;

public class GraphServiceEav : IGraphServiceEav
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;


    public GraphServiceEav(IEntityNodeRepository entityNodeRepository, IGraphNodeRepository graphNodeRepository,IEntityEdgeRepository entityEdgeRepository)
    {
        _entityNodeRepository = entityNodeRepository;
        _graphNodeRepository = graphNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
    }

    public async Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize)
    {
        var valueNodes =  _graphNodeRepository.GetEntityNodesAsync();


        var groupedNodes = valueNodes.Select(g => new PaginationNodeDto
            {
                EntityName = g.Name,
            })
            .ToList(); 
        
        var count = groupedNodes.Count;
        var items = groupedNodes.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PaginatedListDto(items, pageIndex, count, pageSize);
    }
    
    
    public async Task<(IEnumerable<NodeDto> , IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(string id)
    {
        var node =await  _entityNodeRepository.GetEntityByNameAsync(id);
        var edges = await _entityEdgeRepository.FindNodeLoopsAsync(node.Id);
        var uniqueNodes = edges.SelectMany(x => new[] { x.EntityIDTarget , x.EntityIDSource }).Distinct().ToList();
        var nodes = await _entityNodeRepository.GetNodesOfEdgeList(uniqueNodes);
        var nodeDto = nodes.Select(x => new NodeDto() {EntityID = x.Id.ToString() ,EntityName = x.Name});
        var edgeDto = edges.Select(x => new EdgeDto()
            { From = x.EntityIDSource, To = x.EntityIDTarget, EdgeId = x.Id.ToString() });
        return (nodeDto, edgeDto);
    }
}
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Repository;
using AnalysisData.Graph.Services;

namespace AnalysisData.EAV.Service;

public class GraphServiceEav : IGraphServiceEav
{
    private readonly IGraphNodeRepository _graphNodeRepository;

    public GraphServiceEav(IGraphNodeRepository graphNodeRepository)
    {
        _graphNodeRepository = graphNodeRepository;
    }

    public async Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize)
    {
        var valueNodes =  _graphNodeRepository.GetEntityNodesAsync();


        var groupedNodes = valueNodes.Select(g => new NodeDto
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
    
    
    public async Task GetRelationalEdgeBaseNode(int pageIndex, int pageSize)
    {
        var valueNodes =  _graphNodeRepository.GetEntityNodesAsync();


        var groupedNodes = valueNodes.Select(g => new NodeDto
            {
                EntityName = g.Name,
            })
            .ToList(); 
        
        var count = groupedNodes.Count;
        var items = groupedNodes.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

    }
}
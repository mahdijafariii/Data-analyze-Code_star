using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.Graph.Services;

namespace AnalysisData.EAV.Service;

public class GraphServiceEav : IGraphServiceEav
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IEntityNodeRepository _entityNodeRepository;

    public GraphServiceEav(IGraphNodeRepository graphNodeRepository, IEntityNodeRepository entityNodeRepository)
    {
        _graphNodeRepository = graphNodeRepository;
        _entityNodeRepository = entityNodeRepository;
    }

    public async Task<PaginatedListDto> GetNodesAsync(int pageIndex, int pageSize)
    {
        var valueNodes =  _graphNodeRepository.GetValueNodesAsync();


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


    public async Task<Dictionary<string, object>> GetNodeInformation(string headerUniqueId)
    {
        var result = await _graphNodeRepository.GetAttributeValues(headerUniqueId);
         
        var output = new Dictionary<string, object>();

        foreach (var item in result)
        {
            output[item.Attribute] = item.Value;
        }

        return output;
    }

}
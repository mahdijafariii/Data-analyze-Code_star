using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.EdgeRepository.Abstraction;
using AnalysisData.EAV.Repository.NodeRepository.Abstraction;
using AnalysisData.Exception;

namespace AnalysisData.EAV.Service.GraphServices.Relationship;

public class GraphRelationService : IGraphRelationService
{
    private readonly IEntityNodeRepository _entityNodeRepository;
    private readonly IEntityEdgeRepository _entityEdgeRepository;
    private readonly IGraphNodeRepository _graphNodeRepository;


    public GraphRelationService(IEntityNodeRepository entityNodeRepository, IEntityEdgeRepository entityEdgeRepository,
        IGraphNodeRepository graphNodeRepository)
    {
        _entityNodeRepository = entityNodeRepository;
        _entityEdgeRepository = entityEdgeRepository;
        _graphNodeRepository = graphNodeRepository;
    }

    public async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNodeAsync(
        ClaimsPrincipal claimsPrincipal, string id)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        var node = await _entityNodeRepository.GetByIdAsync(id);

        (IEnumerable<NodeDto> nodes, IEnumerable<EdgeDto> edges) result;

        if (role != "dataanalyst")
        {
            result = await GetNodeRelationsAsync(id);
        }
        else if (await _graphNodeRepository.IsNodeAccessibleByUser(username, node.Name))
        {
            result = await GetNodeRelationsAsync(id);
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

    private async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetNodeRelationsAsync(string id)
    {
        var node = await _entityNodeRepository.GetByIdAsync(id);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        var edges = await _entityEdgeRepository.FindNodeLoopsAsync(node.Id);
        var uniqueNodes = edges.SelectMany(x => new[] { x.EntityIDTarget, x.EntityIDSource }).Distinct().ToList();
        var nodes = await _entityNodeRepository.GetEntityNodesByIdsAsync(uniqueNodes);
        var nodeDto = nodes.Select(x => new NodeDto() { Id = x.Id.ToString(), Label = x.Name });
        var edgeDto = edges.Select(x => new EdgeDto()
            { From = x.EntityIDSource, To = x.EntityIDTarget, Id = x.Id.ToString() });
        return (nodeDto, edgeDto);
    }
}
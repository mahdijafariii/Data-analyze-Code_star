using System.Security.Claims;
using AnalysisData.Exception.GraphException;
using AnalysisData.Graph.Dto.EdgeDto;
using AnalysisData.Graph.Dto.NodeDto;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Graph.Repository.EdgeRepository.Abstraction;
using AnalysisData.Graph.Repository.GraphNodeRepository;
using AnalysisData.Graph.Repository.NodeRepository.Abstraction;

namespace AnalysisData.Graph.Service.GraphServices.Relationship;

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
        ClaimsPrincipal claimsPrincipal, int id)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        var node = await _entityNodeRepository.GetByIdAsync(id);

        (IEnumerable<NodeDto> nodes, IEnumerable<EdgeDto> edges) result;
        var usernameGuid = Guid.Parse(username);

        if (role != "dataanalyst")
        {
            result = await GetNodeRelationsAsync(id);
        }
        else if (await _graphNodeRepository.IsNodeAccessibleByUser(usernameGuid, node.Id))
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

    private async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetNodeRelationsAsync(int id)
    {
        var node = await _entityNodeRepository.GetByIdAsync(id);
        if (node is null)
        {
            throw new NodeNotFoundException();
        }

        var edges = await _entityEdgeRepository.FindNodeLoopsAsync(node.Id);
        var uniqueNodes = edges.SelectMany(x => new[] { x.EntityIDTarget, x.EntityIDSource }).Distinct().ToList();
        var nodes = await GetEntityNodesByIdsAsync(uniqueNodes);
        var nodeDto = nodes.Select(x => new NodeDto() { Id = x.Id, Label = x.Name });
        var edgeDto = edges.Select(x => new EdgeDto()
            { From = x.EntityIDSource, To = x.EntityIDTarget, Id = x.Id });
        return (nodeDto, edgeDto);
    }
    
    private async Task<List<EntityNode>> GetEntityNodesByIdsAsync(IEnumerable<int> nodeIdes)
    {
        var entityNodes = new List<EntityNode>();
        foreach (var nodeId in nodeIdes)
        {
            var node = await _entityNodeRepository.GetByIdAsync(nodeId);
            if (nodeId != null)
            {
                entityNodes.Add(node);
            }
        }
        return entityNodes;
    }
}
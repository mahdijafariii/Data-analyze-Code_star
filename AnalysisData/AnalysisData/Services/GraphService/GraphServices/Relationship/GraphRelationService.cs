using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Exception.GraphException.NodeException;
using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;
using AnalysisData.Services.GraphService.GraphServices.Relationship.Abstraction;

namespace AnalysisData.Services.GraphService.GraphServices.Relationship;

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
        ClaimsPrincipal claimsPrincipal, Guid id)
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
            throw new NodeNotAccessibleForUserException();
        }

        if (!result.nodes.Any() && !result.edges.Any())
        {
            throw new NodeNotFoundException();
        }

        return result;
    }

    private async Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetNodeRelationsAsync(Guid id)
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
    
    private async Task<List<EntityNode>> GetEntityNodesByIdsAsync(IEnumerable<Guid> nodeIdes)
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
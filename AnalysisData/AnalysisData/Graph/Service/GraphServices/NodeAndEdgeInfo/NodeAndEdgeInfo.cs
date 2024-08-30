using System.Security.Claims;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.Exception;

namespace AnalysisData.EAV.Service.GraphServices.NodeAndEdgeServices;

public class NodeAndEdgeInfo : INodeAndEdgeInfo
{
    private readonly IGraphNodeRepository _graphNodeRepository;
    private readonly IGraphEdgeRepository _graphEdgeRepository;

    public NodeAndEdgeInfo(IGraphNodeRepository graphNodeRepository, IGraphEdgeRepository graphEdgeRepository)
    {
        _graphNodeRepository = graphNodeRepository;
        _graphEdgeRepository = graphEdgeRepository;
    }

    public async Task<Dictionary<string, string>> GetNodeInformationAsync(ClaimsPrincipal claimsPrincipal,
        int nodeId)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        IEnumerable<dynamic> result = Enumerable.Empty<dynamic>();
        var usernameGuid = Guid.Parse(username);
        if (role != "dataanalyst")
        {
            result = await _graphNodeRepository.GetNodeAttributeValueAsync(nodeId);
        }
        else if (await _graphNodeRepository.IsNodeAccessibleByUser(usernameGuid, nodeId))
        {
            result = await _graphNodeRepository.GetNodeAttributeValueAsync(nodeId);
        }

        if (!result.Any())
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

    public async Task<Dictionary<string, string>> GetEdgeInformationAsync(ClaimsPrincipal claimsPrincipal, int edgeId)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
        var username = claimsPrincipal.FindFirstValue("id");
        var result = Enumerable.Empty<dynamic>();
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
}
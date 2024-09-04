using System.Security.Claims;
using AnalysisData.Graph.Dto.EdgeDto;
using AnalysisData.Graph.Dto.NodeDto;

namespace AnalysisData.Graph.Service.GraphServices.Relationship;

public interface IGraphRelationService
{
    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNodeAsync(ClaimsPrincipal claimsPrincipal,
        int id);
}
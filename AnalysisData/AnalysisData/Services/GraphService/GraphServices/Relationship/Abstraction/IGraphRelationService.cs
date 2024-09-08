using System.Security.Claims;
using AnalysisData.Dtos.GraphDto.EdgeDto;
using AnalysisData.Dtos.GraphDto.NodeDto;

namespace AnalysisData.Services.GraphService.GraphServices.Relationship.Abstraction;

public interface IGraphRelationService
{
    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNodeAsync(
        ClaimsPrincipal claimsPrincipal, Guid id);
}
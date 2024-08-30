using System.Security.Claims;
using AnalysisData.EAV.Dto;

namespace AnalysisData.EAV.Service.GraphServices.Relationship;

public interface IGraphRelationService
{
    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNodeAsync(ClaimsPrincipal claimsPrincipal,
        int id);
}
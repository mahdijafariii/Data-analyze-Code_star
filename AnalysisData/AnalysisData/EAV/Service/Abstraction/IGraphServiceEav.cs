using AnalysisData.EAV.Dto;

namespace AnalysisData.EAV.Service.Abstraction;

public interface IGraphServiceEav
{
    Task<Dictionary<string, string>> GetNodeInformation(string headerUniqueId);
    Task<Dictionary<string, string>> GetEdgeInformation(int edgeId);
    Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize);
    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(string id);
}
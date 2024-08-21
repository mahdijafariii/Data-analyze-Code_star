using AnalysisData.EAV.Dto;

namespace AnalysisData.EAV.Service;

public interface IGraphServiceEav
{
    Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize);
    Task<(IEnumerable<NodeDto>, IEnumerable<EdgeDto>)> GetRelationalEdgeBaseNode(string id);
}
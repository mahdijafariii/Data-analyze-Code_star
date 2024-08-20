using AnalysisData.EAV.Dto;

namespace AnalysisData.EAV.Service;

public interface IGraphServiceEav
{
    Task<PaginatedListDto> GetNodesPaginationAsync(int pageIndex, int pageSize);
}
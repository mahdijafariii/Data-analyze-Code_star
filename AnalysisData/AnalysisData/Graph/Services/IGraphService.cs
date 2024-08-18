using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;

namespace AnalysisData.Graph.Services;

public interface IGraphService
{
    Task<(int, List<PaginationDto>)> GetAllAccountPagination(int page);
}
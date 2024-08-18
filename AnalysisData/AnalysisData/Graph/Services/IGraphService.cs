using AnalysisData.Graph.DataManage.Model;
using AnalysisData.Graph.Dto;

namespace AnalysisData.Graph.Services;

public interface IGraphService
{
    Task<(List<PaginationDto>, int, int)> GetAllAccountPagination(int page = 0);
}
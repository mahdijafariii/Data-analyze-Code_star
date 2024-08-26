using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Dto;


public class PaginationCategoryDto
{
    public List<Category> PaginateList { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }
    public PaginationCategoryDto(List<Category> items, int pageIndex, int totalCount)
    {
        PaginateList = items;
        PageIndex = pageIndex;
        TotalCount = totalCount;
    }
}
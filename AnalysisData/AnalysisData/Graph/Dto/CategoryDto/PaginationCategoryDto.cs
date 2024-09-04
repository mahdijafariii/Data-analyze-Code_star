namespace AnalysisData.Graph.Dto.CategoryDto;

public class PaginationCategoryDto
{
    public List<CategoryDto> PaginateList { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }

    public PaginationCategoryDto(List<CategoryDto> items, int pageIndex, int totalCount)
    {
        PaginateList = items;
        PageIndex = pageIndex;
        TotalCount = totalCount;
    }
}
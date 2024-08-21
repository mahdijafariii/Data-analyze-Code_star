namespace AnalysisData.EAV.Dto;

public class PaginatedListDto 
{
    public List<PaginationNodeDto> Items { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }

    public PaginatedListDto(List<PaginationNodeDto> items, int pageIndex, int totalCount, int pageSize)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalCount = totalCount;
    }    
}
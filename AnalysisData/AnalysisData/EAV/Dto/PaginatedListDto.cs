namespace AnalysisData.EAV.Dto;

public class PaginatedListDto 
{
    public List<NodeDto> Items { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }

    public PaginatedListDto(List<NodeDto> items, int pageIndex, int totalCount, int pageSize)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalCount = totalCount;
    }    
}
namespace AnalysisData.EAV.Dto;

public class PaginatedListDto 
{
    public List<string> PaginateList { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }
    public IEnumerable<string> TypeCategory { get; }

    public PaginatedListDto(List<string> items, int pageIndex, int totalCount, IEnumerable<string> typeCategory)
    {
        PaginateList = items;
        PageIndex = pageIndex;
        TotalCount = totalCount;
        TypeCategory = typeCategory;

    }    
}
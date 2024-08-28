namespace AnalysisData.EAV.Dto;

public class PaginatedNodeListDto 
{
    public List<string> PaginateList { get; }
    public int PageIndex { get; }
    public int TotalCount { get; }
    public string TypeCategory { get; }

    public PaginatedNodeListDto(List<string> items, int pageIndex, int totalCount, string typeCategory)
    {
        PaginateList = items;
        PageIndex = pageIndex;
        TotalCount = totalCount;
        TypeCategory = typeCategory;
    }    
}
namespace AnalysisData.Graph.Dto.NodeDto;

public class PaginatedNodeListDto
{
    public List<PaginationNodeDto> Items { get; set; }
    public int PageIndex { get; set; }
    public int TotalItems { get; set; }
    public string CategoryName { get; set; }

    public PaginatedNodeListDto(List<PaginationNodeDto> items, int pageIndex, int totalItems, string categoryName)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalItems = totalItems;
        CategoryName = categoryName;
    }
}
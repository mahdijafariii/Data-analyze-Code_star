namespace AnalysisData.Graph.Dto;

public class PaginatedFileDto
{
    public List<FileEntityDto> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }

    public PaginatedFileDto(List<FileEntityDto> items, int totalCount, int pageIndex)
    {
        Items = items;
        TotalCount = totalCount;
        PageIndex = pageIndex;
    }
}
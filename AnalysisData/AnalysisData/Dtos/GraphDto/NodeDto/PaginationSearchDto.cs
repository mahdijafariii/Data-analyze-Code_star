using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Dtos.GraphDto.NodeDto;

public class PaginationSearchDto
{
    public List<PaginationNodeDto> Items { get; set; }
    public int PageIndex { get; set; }
    public int TotalItems { get; set; }

    public PaginationSearchDto(List<PaginationNodeDto> items, int pageIndex, int totalItems)
    {
        Items = items;
        PageIndex = pageIndex;
        TotalItems = totalItems;
    }
}
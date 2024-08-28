namespace AnalysisData.EAV.Dto;

public class PaginatedFilesDto
{
    public List<UploadDataDto> Items { get; set; }
    public int TotalCount { get; set; }
    public int PageIndex { get; set; }
}
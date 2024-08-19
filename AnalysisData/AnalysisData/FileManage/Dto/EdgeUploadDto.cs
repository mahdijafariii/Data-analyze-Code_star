namespace AnalysisData.FileManage.Dto;

public class EdgeUploadDto
{
    public string From { get; set; }
    public string To { get; set; }
    public IFormFile File { get; set; }
}
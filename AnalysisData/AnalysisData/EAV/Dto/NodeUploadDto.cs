using System.ComponentModel.DataAnnotations;

namespace AnalysisData.FileManage.Dto;

public class NodeUploadDto
{
    [Required]
    public string Header { get; set; }
    [Required]
    public IFormFile File { get; set; }
}
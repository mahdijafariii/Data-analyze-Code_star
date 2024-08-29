using System.ComponentModel.DataAnnotations;
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Dto;

public class NodeUploadDto
{
    [Required] public string Header { get; set; }
    [Required] public IFormFile File { get; set; }
    [Required] public int CategoryId { get; set; }
    public string Name { get; set; }
}
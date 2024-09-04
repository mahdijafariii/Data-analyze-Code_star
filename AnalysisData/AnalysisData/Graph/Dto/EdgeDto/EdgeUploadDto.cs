using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Graph.Dto.EdgeDto;

public class EdgeUploadDto
{
    [Required] public string From { get; set; }
    [Required] public string To { get; set; }
    [Required] public IFormFile File { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace AnalysisData.EAV.Dto;

public class NewCategoryDto
{
    [Required]
    public string Name { get; set; }
}
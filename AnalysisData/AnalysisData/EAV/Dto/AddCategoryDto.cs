using System.ComponentModel.DataAnnotations;

namespace AnalysisData.EAV.Dto;

public class AddCategoryDto
{
    [Required]
    public string Name { get; set; }
}
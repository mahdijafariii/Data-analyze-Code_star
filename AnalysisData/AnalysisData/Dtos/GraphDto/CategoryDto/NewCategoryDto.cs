using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Dtos.GraphDto.CategoryDto;

public class NewCategoryDto
{
    [Required] public string Name { get; set; }
}
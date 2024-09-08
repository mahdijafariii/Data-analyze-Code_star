using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Dtos.GraphDto.CategoryDto;

public class NewCategoryDto
{
    [Required] public int Id { get; set; }
    [Required] public string Name { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Graph.Dto.CategoryDto;

public class NewCategoryDto
{
    [Required] public string Name { get; set; }
}
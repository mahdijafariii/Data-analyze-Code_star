using System.ComponentModel.DataAnnotations;

namespace AnalysisData.EAV.Model;

public class EntityEdge
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int EntityIDSource { get; set; }

    [Required]
    public int EntityIDTarget { get; set; }
}
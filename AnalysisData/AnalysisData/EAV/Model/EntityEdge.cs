using System.ComponentModel.DataAnnotations;

namespace AnalysisData.EAV.Model;

public class EntityEdge
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string EntityIDSource { get; set; }

    [Required]
    public string EntityIDTarget { get; set; }
}
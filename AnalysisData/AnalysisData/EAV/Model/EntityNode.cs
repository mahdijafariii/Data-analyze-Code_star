using System.ComponentModel.DataAnnotations;

namespace AnalysisData.EAV.Model;

public class EntityNode
{
    [Key]
    public int Id { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
}
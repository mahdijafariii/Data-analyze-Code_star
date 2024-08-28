using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalysisData.EAV.Model;

public class EntityNode
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    
    public int NodeFileReferenceId { get; set; }
    
    [ForeignKey("NodeFileReferenceId")]
    public FileEntity FileEntity { get; set; }
}
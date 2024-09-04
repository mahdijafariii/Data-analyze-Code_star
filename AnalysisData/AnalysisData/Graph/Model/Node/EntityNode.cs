using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.Graph.Model.File;

namespace AnalysisData.Graph.Model.Node;

public class EntityNode
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; }

    public int NodeFileReferenceId { get; set; }

    [ForeignKey("NodeFileReferenceId")] public FileEntity FileEntity { get; set; }
}
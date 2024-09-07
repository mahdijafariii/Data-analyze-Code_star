using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Models.GraphModel.Edge;

public class EntityEdge
{
    [Key] 
    public Guid Id { get; set; }

    [Required] 
    public Guid EntityIDSource { get; set; }

    [ForeignKey("EntityIDSource")]
    public EntityNode SourceNode { get; set; }

    [Required] 
    public Guid EntityIDTarget { get; set; }

    [ForeignKey("EntityIDTarget")]
    public EntityNode TargetNode { get; set; }
}
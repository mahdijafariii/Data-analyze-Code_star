using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalysisData.Graph.Model.Node;

public class ValueNode
{
    [Key] public int Id { get; set; }
    public string Value { get; set; }

    [Required] [ForeignKey("Entity")] public int EntityId { get; set; }

    [Required] [ForeignKey("Attribute")] public int AttributeId { get; set; }
    public EntityNode Entity { get; set; }
    public AttributeNode Attribute { get; set; }
}
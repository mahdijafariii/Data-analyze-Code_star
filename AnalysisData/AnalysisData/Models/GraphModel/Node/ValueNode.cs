using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalysisData.Models.GraphModel.Node;

public class ValueNode
{
    [Key] public Guid Id { get; set; }
    public string Value { get; set; }

    [Required] [ForeignKey("Entity")] public Guid EntityId { get; set; }

    [Required] [ForeignKey("Attribute")] public Guid AttributeId { get; set; }
    public EntityNode Entity { get; set; }
    public AttributeNode Attribute { get; set; }
}
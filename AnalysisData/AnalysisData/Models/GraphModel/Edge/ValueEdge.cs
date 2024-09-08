using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Models.GraphModel.Edge;

public class ValueEdge
{
    [Key] public Guid Id { get; set; }

    public Guid EntityId { get; set; }
    public Guid AttributeId { get; set; }

    [Required] public string Value { get; set; }

    public EntityEdge Entity { get; set; }
    public AttributeEdge Attribute { get; set; }
}
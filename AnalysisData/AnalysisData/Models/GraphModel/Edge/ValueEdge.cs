using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Models.GraphModel.Edge;

public class ValueEdge
{
    [Key] public int Id { get; set; }

    public int EntityId { get; set; }
    public int AttributeId { get; set; }

    [Required] public string Value { get; set; }

    public EntityEdge Entity { get; set; }
    public AttributeEdge Attribute { get; set; }
}
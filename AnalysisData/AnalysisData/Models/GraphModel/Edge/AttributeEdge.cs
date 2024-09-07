using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Models.GraphModel.Edge;

public class AttributeEdge
{
    [Key] 
    public Guid Id { get; set; }
    public string Name { get; set; }
}
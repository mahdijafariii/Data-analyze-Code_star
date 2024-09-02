using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Graph.Model.Edge;

public class AttributeEdge
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Graph.Model.Node;

public class AttributeNode
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
}
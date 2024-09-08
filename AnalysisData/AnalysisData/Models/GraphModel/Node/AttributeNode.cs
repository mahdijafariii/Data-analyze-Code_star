using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Models.GraphModel.Node;

public class AttributeNode
{
    [Key] 
    public Guid Id { get; set; }
    public string Name { get; set; }
}
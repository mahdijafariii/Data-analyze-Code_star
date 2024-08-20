using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalysisData.EAV.Model;

public class ValueNode
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Entity")]
    public int EntityId { get; set; }

    [Required]
    [ForeignKey("Attribute")]
    public int AttributeId { get; set; }

    public string ValueString { get; set; }

    // Navigation properties
    public EntityNode Entity { get; set; }
    public AttributeNode Attribute { get; set; }
}
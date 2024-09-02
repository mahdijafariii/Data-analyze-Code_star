using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.Graph.Model.Node;
using AnalysisData.Model;

namespace AnalysisData.Graph.Model.File;

public class FileEntity
{
    [Key] public int Id { get; set; }

    public Guid UploaderId { get; set; }

    [ForeignKey("UploaderId")] public User User { get; set; }

    [Required] public DateTime UploadDate { get; set; }
    public string FileName { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")] public Category.Category Category { get; set; }

    public ICollection<EntityNode> EntityNodes { get; set; }
}
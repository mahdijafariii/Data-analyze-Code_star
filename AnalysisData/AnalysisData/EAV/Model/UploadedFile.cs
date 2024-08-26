using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Model;

public class UploadedFile
{
    [Key] 
    public int Id { get; set; }
    
    public Guid UploaderId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }
    [Required]
    public DateTime UploadDate { get; set; }
    public string Category { get; set; }
    public string FileName { get; set; }
    public ICollection<EntityNode> EntityNodes { get; set; }
}
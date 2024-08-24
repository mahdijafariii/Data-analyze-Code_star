using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Model;

public class UploadData
{
    [Key] 
    public int Id { get; set; }
    
    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    public string Category { get; set; }

    // Navigation property for the one-to-many relationship with UserFile
    public ICollection<UserFile> UserFiles { get; set; }
}
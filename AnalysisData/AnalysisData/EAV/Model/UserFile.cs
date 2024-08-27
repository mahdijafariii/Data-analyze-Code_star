using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Model;

public class UserFile
{
    [Key] public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public Guid FileId { get; set; }
    
    [ForeignKey("UploadDataId")]
    public UploadedFile UploadedFile { get; set; }
}
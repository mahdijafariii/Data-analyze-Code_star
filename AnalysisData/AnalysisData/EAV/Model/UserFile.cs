using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Model;

public class UserFile
{
    [Key]
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [ForeignKey("UserId")]
    public User User { get; set; }
    
    public int UploadDataId { get; set; }
    
    [ForeignKey("UploadDataId")]
    public UploadData UploadData { get; set; }
}
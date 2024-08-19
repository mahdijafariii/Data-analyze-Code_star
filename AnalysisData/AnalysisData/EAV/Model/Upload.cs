using System.ComponentModel.DataAnnotations;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Model;

public class Upload
{
    [Key]
    public int Id { get; set; }
    public string Content { get; set; }
    
    public User User { get; set; }
    public Guid UserId { get; set; }

}
    
    

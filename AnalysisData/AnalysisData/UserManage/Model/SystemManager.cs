using System.ComponentModel.DataAnnotations;

namespace AnalysisData.UserManage.Model;

public class SystemManager
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
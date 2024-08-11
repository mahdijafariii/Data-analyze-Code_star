using System.ComponentModel.DataAnnotations;

namespace AnalysisData.UserManage.Model;

public class DataAnalyst
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}//guid

using System.ComponentModel.DataAnnotations;

namespace AnalysisData.UserManage.Model;

public class Role
{
    [Key]
    public int Id { get; set; }

    public string RoleName { get; set; }
}
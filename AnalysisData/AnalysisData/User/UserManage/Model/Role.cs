namespace AnalysisData.UserManage.Model;
using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public int Id { get; set; }

    public string RoleName { get; set; }
    public string RolePolicy { get; set; }
}
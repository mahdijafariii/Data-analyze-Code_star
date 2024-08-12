using System.ComponentModel.DataAnnotations;

namespace AnalysisData.UserManage.Model;

public class Role
{
    [Key] public int Id { get; set; } //guid
    public string RoleType { get; set; }
    public UserRole UserRole { get; set; }
}
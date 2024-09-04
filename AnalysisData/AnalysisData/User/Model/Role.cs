using System.ComponentModel.DataAnnotations;

namespace AnalysisData.User.Model;

public class Role
{
    [Key] public int Id { get; set; }

    public string RoleName { get; set; }
    public string RolePolicy { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace AnalysisData.Models.UserModel;

public class Role
{
    [Key] public int Id { get; set; }

    public string RoleName { get; set; }
    public string RolePolicy { get; set; }
}
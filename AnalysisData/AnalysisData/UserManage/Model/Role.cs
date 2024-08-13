using System.ComponentModel.DataAnnotations;

namespace AnalysisData.UserManage.Model;

public class Role
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string RoleName { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
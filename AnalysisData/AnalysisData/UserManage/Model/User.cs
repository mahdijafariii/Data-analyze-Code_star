using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.UserManage.Model;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public ICollection<UserRole> UserRole { get; set; }
}
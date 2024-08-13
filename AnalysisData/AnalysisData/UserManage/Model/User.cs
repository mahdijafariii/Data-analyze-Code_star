using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.UserManage.Model;


public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [StringLength(256)]
    public string Password { get; set; } 

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}


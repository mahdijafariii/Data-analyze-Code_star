using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AnalysisData.UserManage.Model;

public class User
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public string Username { get; set; }

    [Required] [StringLength(256)] public string Password { get; set; }

    [Required] [StringLength(50)] public string FirstName { get; set; }

    [Required] [StringLength(50)] public string LastName { get; set; }

    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")] // not work
    public string PhoneNumber { get; set; }

    public string? ImageURL { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
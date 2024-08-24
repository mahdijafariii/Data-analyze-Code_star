using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnalysisData.EAV.Model;

namespace AnalysisData.UserManage.Model;

public class User
{
    [Key] public Guid Id { get; set; } = new Guid();

    public string Username { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string? ImageURL { get; set; }
    public string Role { get; set; }
    
    // Navigation property for the one-to-many relationship with UploadData
    public ICollection<UploadData> UploadData { get; set; }
    
    // Navigation property for the one-to-many relationship with UserFile
    public ICollection<UserFile> UserFiles { get; set; }
    
}
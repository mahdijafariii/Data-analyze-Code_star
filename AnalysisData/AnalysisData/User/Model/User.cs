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

    public ICollection<FileEntity> UploadData { get; set; }

    public ICollection<UserFile> UserFiles { get; set; }

    [ForeignKey("RoleId")] public int RoleId { get; set; }
    public Role Role { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalysisData.Models.UserModel;

public class PasswordResetToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } 
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsUsed = false;
}
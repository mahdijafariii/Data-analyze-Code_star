using System.ComponentModel.DataAnnotations.Schema;

namespace AnalysisData.User.Model;

public class ResetPasswordToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; } 
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
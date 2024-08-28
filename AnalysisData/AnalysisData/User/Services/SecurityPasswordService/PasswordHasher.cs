using System.Security.Cryptography;
using System.Text;
using AnalysisData.Services.SecurityPasswordService.Abstraction;

namespace AnalysisData.Services.SecurityPasswordService;

public class PasswordHasher:IPasswordHasher
{
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }

}
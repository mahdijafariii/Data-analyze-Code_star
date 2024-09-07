using System.Security.Cryptography;
using System.Text;
using AnalysisData.Exception.PasswordException;
using AnalysisData.Services.UserService.UserService.Business.Abstraction;

namespace AnalysisData.Services.UserService.UserService.Business;

public class PasswordHasherManager : IPasswordHasherManager
{
    public string HashPassword(string password)
    {
        if (password is null)
        {
            throw new PasswordHasherInputNull();
        }

        using var sha256 = SHA256.Create();
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = sha256.ComputeHash(passwordBytes);
        return Convert.ToBase64String(hashBytes);
    }
}
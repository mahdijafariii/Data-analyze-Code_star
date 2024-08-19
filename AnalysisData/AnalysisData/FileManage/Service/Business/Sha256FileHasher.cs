/*using System.Security.Cryptography;

namespace AnalysisData.FileManage.Service.Business;

public class Sha256FileHasher : IFileHasher
{
    public string Hash(Stream fileStream)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(fileStream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}*/
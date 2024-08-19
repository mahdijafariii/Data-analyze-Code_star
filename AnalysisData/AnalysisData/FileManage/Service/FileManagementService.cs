using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.Exception;
using AnalysisData.FileManage.Service.Abstraction;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.FileManage.Service;


public class FileManagementService : IFileManagementService
{
    private readonly IUploadRepository _uploadRepository;

    public FileManagementService(IUploadRepository uploadRepository)
    {
        _uploadRepository = uploadRepository;
    }

    public async Task<string> FileUpload(ClaimsPrincipal claimsPrincipal, Stream fileStream)
    {
        var id = claimsPrincipal.FindFirstValue("id");

        if (!Guid.TryParse(id, out var userId))
        {
            throw new ArgumentException("Invalid user ID in token");
        }
        
        using var fileStreamCopy = new MemoryStream();
        await fileStream.CopyToAsync(fileStreamCopy);
        fileStreamCopy.Seek(0, SeekOrigin.Begin);
    

        using var reader = new StreamReader(fileStreamCopy);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        });
        
        
        csv.Read();
        csv.ReadHeader();
    
        var hashString = HashContentFile(fileStreamCopy);
        var existFile= _uploadRepository.GetByContentAsync(hashString);
        if (existFile != null)
            throw new FileExistenceException();

        var temp = new Upload
        {
            UserId = userId,
            Content = hashString,
        };

        await _uploadRepository.AddAsync(temp);

        return string.Join("-", csv.HeaderRecord);
    }

    private static string HashContentFile(MemoryStream fileStreamCopy)
    {
        var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(fileStreamCopy);
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hashString;
    }
}
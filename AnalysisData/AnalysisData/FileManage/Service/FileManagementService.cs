/*using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.Exception;
using AnalysisData.FileManage.Service.Abstraction;
using AnalysisData.FileManage.Service.Business;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace AnalysisData.FileManage.Service;


public class FileManagementService : IFileManagementService
{
    private readonly IUploadRepository _uploadRepository;
    private readonly IFileHasher _fileHasher;
    private readonly ICsvProcessor _csvProcessor;

    public FileManagementService(IUploadRepository uploadRepository, IFileHasher fileHasher, ICsvProcessor csvProcessor)
    {
        _uploadRepository = uploadRepository;
        _fileHasher = fileHasher;
        _csvProcessor = csvProcessor;
    }

    public async Task<(string, string)> FileUpload(ClaimsPrincipal claimsPrincipal, Stream fileStream)
    {
        var userId = GetUserIdFromClaims(claimsPrincipal);
        using var fileStreamCopy = new MemoryStream();
        await CopyStreamAsync(fileStream, fileStreamCopy);

        var hashString = _fileHasher.Hash(fileStreamCopy);
        await CheckFileExistenceAsync(hashString);

        var csvHeader = _csvProcessor.ReadHeader(fileStreamCopy);

        var newFile = new Upload
        {
            UserId = userId,
            Content = hashString,
        };

        await _uploadRepository.AddAsync(newFile);

        return (csvHeader, newFile.Id.ToString());
    }

    private static Guid GetUserIdFromClaims(ClaimsPrincipal claimsPrincipal)
    {
        var id = claimsPrincipal.FindFirstValue("id");
        if (!Guid.TryParse(id, out var userId))
        {
            throw new ArgumentException("Invalid user ID in token");// exception
        }
        return userId;
    }

    private static async Task CopyStreamAsync(Stream sourceStream, Stream targetStream)
    {
        await sourceStream.CopyToAsync(targetStream);
        targetStream.Seek(0, SeekOrigin.Begin);
    }

    private async Task CheckFileExistenceAsync(string hashString)
    {
        var existFile = await _uploadRepository.GetByContentAsync(hashString);
        if (existFile != null)
        {
            throw new FileExistenceException();
        }
    }
}*/
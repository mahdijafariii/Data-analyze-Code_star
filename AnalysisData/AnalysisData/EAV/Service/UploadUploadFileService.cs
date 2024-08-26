using System.Security.Claims;
using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.EAV.Service.Abstraction;

namespace AnalysisData.EAV.Service;

public class UploadUploadFileService : IUploadFileService
{
    private readonly IUploadDataRepository _uploadDataRepository;

    public UploadUploadFileService(IUploadDataRepository uploadDataRepository)
    {
        _uploadDataRepository = uploadDataRepository;
    }

    public async Task<int> AddFileToDb(string category, ClaimsPrincipal claimsPrincipal, string name)
    {
        var guid = Guid.Parse(claimsPrincipal.FindFirstValue("id"));
        var uploadData = new UploadData
        {
            UserId = guid,
            Category = category,
            Name = name,
            UploadDate = DateTime.UtcNow
        };
        await _uploadDataRepository.AddAsync(uploadData);
        return uploadData.Id;
    }
    

}
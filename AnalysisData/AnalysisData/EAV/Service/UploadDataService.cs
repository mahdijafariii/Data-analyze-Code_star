using System.Security.Claims;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Service.Abstraction;

namespace AnalysisData.EAV.Service;

public class UploadDataService : IUploadFileService
{
    private readonly IUploadDataRepository _uploadDataRepository;

    public UploadDataService(IUploadDataRepository uploadDataRepository)
    {
        _uploadDataRepository = uploadDataRepository;
    }

    public async Task<int> AddFileToDb(string category, ClaimsPrincipal claimsPrincipal)
    {
        var guid = Guid.Parse(claimsPrincipal.FindFirstValue("id"));
        var uploadData = new UploadData
        {
            UserId = guid,
            Category = category,
            UploadDate = DateTime.UtcNow
        };
        await _uploadDataRepository.AddAsync(uploadData);
        return uploadData.Id;
    }
}
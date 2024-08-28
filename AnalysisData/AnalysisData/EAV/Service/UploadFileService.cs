using System.Security.Claims;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Service.Abstraction;

namespace AnalysisData.EAV.Service;

public class UploadFileService : IUploadFileService
{
    private readonly IUploadDataRepository _uploadDataRepository;

    public UploadFileService(IUploadDataRepository uploadDataRepository)
    {
        _uploadDataRepository = uploadDataRepository;
    }

    public async Task<int> AddFileToDb(int categoryId, ClaimsPrincipal claimsPrincipal, string name)
    {
        var guid = Guid.Parse(claimsPrincipal.FindFirstValue("id"));
        var uploadData = new FileEntity
        {
            UploaderId = guid,
            CategoryId = categoryId,
            FileName = name,
            UploadDate = DateTime.UtcNow
        };
        await _uploadDataRepository.AddAsync(uploadData);
        return uploadData.Id;
    }
    

}
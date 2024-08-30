using System.Security.Claims;
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Repository.FileUploadedRepository;
using AnalysisData.EAV.Service.Abstraction;

namespace AnalysisData.EAV.Service;

public class UploadFileService : IUploadFileService
{
    private readonly IFileUploadedRepository _uploadedRepository;

    public UploadFileService(IFileUploadedRepository uploadedRepository)
    {
        _uploadedRepository = uploadedRepository;
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
        await _uploadedRepository.AddAsync(uploadData);
        return uploadData.Id;
    }
}
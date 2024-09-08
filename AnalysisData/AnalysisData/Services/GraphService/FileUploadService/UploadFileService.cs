using System.Security.Claims;
using AnalysisData.Exception.GraphException.CategoryException;
using AnalysisData.Models.GraphModel.File;
using AnalysisData.Repositories.GraphRepositories.CategoryRepository.Abstraction;
using AnalysisData.Repositories.GraphRepositories.FileUploadedRepository.Abstraction;
using AnalysisData.Services.GraphService.FileUploadService.Abstraction;

namespace AnalysisData.Services.GraphService.FileUploadService;

public class UploadFileService : IUploadFileService
{
    private readonly IFileUploadedRepository _uploadedRepository;
    private readonly ICategoryRepository _categoryRepository; 

    public UploadFileService(IFileUploadedRepository uploadedRepository, ICategoryRepository categoryRepository)
    {
        _uploadedRepository = uploadedRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<int> AddFileToDb(int categoryId, ClaimsPrincipal claimsPrincipal, string name)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId);
        if (category == null)
        {
            throw new CategoryResultNotFoundException();
        }
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
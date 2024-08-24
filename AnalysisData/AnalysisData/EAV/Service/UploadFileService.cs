
using AnalysisData.EAV.Model;
using AnalysisData.EAV.Repository;
using AnalysisData.EAV.Repository.Abstraction;
using AnalysisData.EAV.Service.Abstraction;
using AnalysisData.Exception;

namespace AnalysisData.EAV.Service;

public class UploadFileService : IUploadFileService
{
    private readonly IUploadFileRepository _uploadFileRepository;

    public UploadFileService(IUploadFileRepository uploadFileRepository)
    {
        _uploadFileRepository = uploadFileRepository;
    }

    public async Task AddFileToDb(IFormFile file)
    {
        var name = file.Name;
        var check = await _uploadFileRepository.GetByNameAsync(name);
        if (check != null)
        {
            throw new FileExistenceException();
        }

        var newFile = new UploadData { Name = name };
        await _uploadFileRepository.AddAsync(newFile);
    }
}
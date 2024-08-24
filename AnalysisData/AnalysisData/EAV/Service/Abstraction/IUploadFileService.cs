namespace AnalysisData.EAV.Service.Abstraction;

public interface IUploadFileService
{
    Task AddFileToDb(IFormFile file);
}
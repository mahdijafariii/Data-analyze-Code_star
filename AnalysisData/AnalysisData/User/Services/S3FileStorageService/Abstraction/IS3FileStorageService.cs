namespace AnalysisData.Services.S3FileStorageService;

public interface IS3FileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
}
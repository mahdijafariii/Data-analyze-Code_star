namespace AnalysisData.User.Services.S3FileStorageService.Abstraction;

public interface IS3FileStorageService
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
}
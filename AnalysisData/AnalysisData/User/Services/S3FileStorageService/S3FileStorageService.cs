using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AnalysisData.Services.S3FileStorageService.Abstraction;

namespace AnalysisData.Services.S3FileStorageService;

public class S3FileStorageService : IS3FileStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3FileStorageService(IConfiguration configuration)
    {
        _bucketName = configuration["AWS:BucketName"];
        _s3Client = new AmazonS3Client(
            configuration["AWS:AccessKey"],
            configuration["AWS:SecretKey"],
            RegionEndpoint.GetBySystemName(configuration["AWS:Region"])
        );
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folderName)
    {
        var fileKey = Path.Combine(folderName, file.FileName).Replace("\\", "/");

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileKey,
            InputStream = file.OpenReadStream(),
            ContentType = file.ContentType
        };

        await _s3Client.PutObjectAsync(putRequest);

        return $"https://{_bucketName}.s3.amazonaws.com/{fileKey}";
    }
}
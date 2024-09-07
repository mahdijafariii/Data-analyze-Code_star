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
        var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(configuration["AWS:AccessKey"], configuration["AWS:SecretKey"]);
        var config = new AmazonS3Config
        {
            ServiceURL = configuration["AWS:ServiceURL"] 
        };
        _s3Client = new AmazonS3Client(awsCredentials, config);
        _bucketName = configuration["AWS:BucketName"];
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

        var response = await _s3Client.PutObjectAsync(putRequest);
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            var fileUrl = $"https://{_bucketName}.s3.{RegionEndpoint.USEast1.SystemName}.amazonaws.com/{fileKey}";
            return fileUrl;        
        }
        else
        {
            return "Could not upload .";
        }
    }
}
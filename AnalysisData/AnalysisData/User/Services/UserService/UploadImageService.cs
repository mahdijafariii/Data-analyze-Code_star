using System.Security.Claims;
using AnalysisData.Exception;
using AnalysisData.Repository.UserRepository.Abstraction;
using AnalysisData.Services.S3FileStorageService;

namespace AnalysisData.Services;

public class UploadImageService
{
    private readonly IUserRepository _userRepository;
    private readonly IS3FileStorageService _s3FileStorageService;

    public UploadImageService(IUserRepository userRepository, IS3FileStorageService s3FileStorageService)
    {
        _userRepository = userRepository;
        _s3FileStorageService = s3FileStorageService;
    }

    public async Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, IFormFile file)
    {
        var userName = claimsPrincipal.FindFirstValue("username");
        var user = await _userRepository.GetUserByUsernameAsync(userName);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        string imageUrl = null; 
        if (file != null && file.Length > 0)
        {
            imageUrl = await _s3FileStorageService.UploadFileAsync(file, "UserImages");
            user.ImageURL = imageUrl;
        }
        user.ImageURL = imageUrl;
        await _userRepository.UpdateUserAsync(user.Id, user);
        return true;
    }
}
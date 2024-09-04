using System.Security.Claims;

namespace AnalysisData.User.Services.UserService.Abstraction;

public interface IUploadImageService
{
    Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, IFormFile file);
}
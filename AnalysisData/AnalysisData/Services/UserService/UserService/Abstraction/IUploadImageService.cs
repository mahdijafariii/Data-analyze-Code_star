using System.Security.Claims;

namespace AnalysisData.Services.UserService.UserService.Abstraction;

public interface IUploadImageService
{
    Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, IFormFile file);
}
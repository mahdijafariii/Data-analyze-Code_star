using System.Security.Claims;

namespace AnalysisData.Services.Abstraction;

public interface IUploadImageService
{
    Task<bool> UploadImageAsync(ClaimsPrincipal claimsPrincipal, IFormFile file);
}
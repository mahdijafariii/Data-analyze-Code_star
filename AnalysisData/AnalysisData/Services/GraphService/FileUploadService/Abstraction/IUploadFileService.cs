using System.Security.Claims;

namespace AnalysisData.Services.GraphService.FileUploadService.Abstraction;

public interface IUploadFileService
{
    Task<int> AddFileToDb(int categoryID, ClaimsPrincipal claimsPrincipal, string name);
}
using System.Security.Claims;

namespace AnalysisData.Graph.Service.FileUploadService.Abstraction;

public interface IUploadFileService
{
    Task<int> AddFileToDb(int categoryID, ClaimsPrincipal claimsPrincipal, string name);
}
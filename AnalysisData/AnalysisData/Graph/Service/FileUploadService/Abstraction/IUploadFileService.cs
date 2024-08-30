using System.Security.Claims;

namespace AnalysisData.EAV.Service.Abstraction;

public interface IUploadFileService
{
    Task<int> AddFileToDb(int categoryID, ClaimsPrincipal claimsPrincipal, string name);
}
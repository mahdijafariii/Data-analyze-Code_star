using System.Security.Claims;

namespace AnalysisData.EAV.Service.Abstraction;

public interface IUploadFileService
{
    Task AddFileToDb(string category, ClaimsPrincipal claimsPrincipal);
}
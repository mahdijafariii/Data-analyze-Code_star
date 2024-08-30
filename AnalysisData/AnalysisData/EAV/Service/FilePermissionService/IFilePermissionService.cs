using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Service;

public interface IFilePermissionService
{
    Task<PaginatedFileDto> GetFilesAsync(int page, int limit);
    Task<List<UserAccessDto>> GetUserForAccessingFileAsync(string username);
    Task<IEnumerable<UserFile>> WhoAccessThisFileAsync(string fileId);
    Task AccessFileToUserAsync(List<string> inputUserIds, int fileId);
}
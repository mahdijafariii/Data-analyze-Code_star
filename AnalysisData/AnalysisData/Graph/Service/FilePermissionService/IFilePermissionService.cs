using AnalysisData.Graph.Dto;

namespace AnalysisData.Graph.Service.FilePermissionService;

public interface IFilePermissionService
{
    Task<PaginatedFileDto> GetFilesAsync(int page, int limit);
    Task<List<UserAccessDto>> GetUserForAccessingFileAsync(string username);
    Task<IEnumerable<WhoAccessThisFileDto>> WhoAccessThisFileAsync(int fileId);
    Task AccessFileToUserAsync(List<string> inputUserIds, int fileId);
}
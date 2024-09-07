using AnalysisData.Dtos.GraphDto.FileDto;
using AnalysisData.Dtos.UserDto.UserDto;

namespace AnalysisData.Services.GraphService.FilePermissionService.Abstractions;

public interface IFilePermissionService
{
    Task<PaginatedFileDto> GetFilesAsync(int page, int limit);
    Task<List<UserAccessDto>> GetUserForAccessingFileAsync(string username);
    Task<IEnumerable<WhoAccessThisFileDto>> WhoAccessThisFileAsync(int fileId);
    Task AccessFileToUserAsync(List<string> inputUserIds, int fileId);
}
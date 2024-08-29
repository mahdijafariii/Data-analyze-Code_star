using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Service;

public interface IFilePermissionService
{
    Task<PaginatedFilesDto> GetFilesPagination(int page, int limit);
    Task<List<UserAccessDto>> GetUserForAccessingFile(string username);
    Task<IEnumerable<UserWhoAccessThisFileDto>> WhoAccessThisFile(int fileId);
    Task AccessFileToUser(List<string> inputUserIds, int fileId);
}
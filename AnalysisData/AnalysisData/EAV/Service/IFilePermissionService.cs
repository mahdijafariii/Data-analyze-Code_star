using AnalysisData.EAV.Dto;
using AnalysisData.EAV.Model;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Service;

public interface IFilePermissionService
{
    Task<List<FileEntityDto>> GetFilesPagination(int page, int limit);
    Task<List<UserAccessDto>> GetUserForAccessingFile(string username);
    Task<IEnumerable<UserFile>> WhoAccessThisFile(string fileId);
    Task AccessFileToUser(List<string> inputUserIds, int fileId);
}
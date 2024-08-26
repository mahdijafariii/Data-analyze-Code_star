using AnalysisData.EAV.Dto;
using AnalysisData.UserManage.Model;

namespace AnalysisData.EAV.Service;

public interface IFilePermissionService
{
    Task<List<UploadDataDto>> GetFilesPagination(int page, int limit);
    Task<List<User>> GetUserForAccessingFile(string username);
}
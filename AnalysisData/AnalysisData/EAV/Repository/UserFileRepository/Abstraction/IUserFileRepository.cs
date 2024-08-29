using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUserFileRepository
{
    Task AddAsync(UserFile userFile);
    Task<IEnumerable<UserFile>> GetAllAsync();
    Task<UserFile?> GetByUserIdAsync(string userId);
    Task<IEnumerable<string>> GetUserIdsWithAccessToFileAsync(string fileId);
    Task<IEnumerable<UserFile?>> GetByFileIdAsync(string fileId);
    Task DeleteByUserIdAsync(string userId);
    Task RevokeUserAccessAsync(List<string> userIds);
    Task GrantUserAccessAsync(List<string> userIds, int fileId);
}
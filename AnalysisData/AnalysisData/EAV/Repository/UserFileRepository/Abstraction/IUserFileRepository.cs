using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUserFileRepository
{
    Task AddAsync(UserFile userFile);
    Task<IEnumerable<UserFile>> GetAllAsync();
    Task<UserFile> GetByUserIdAsync(string userId);
    Task<IEnumerable<string>> GetUserIdsWithAccessToFileAsync(string fileId);
    Task<IEnumerable<UserFile>> GetByFileIdAsync(int fileId);
    Task DeleteByUserIdAsync(string userId);
}
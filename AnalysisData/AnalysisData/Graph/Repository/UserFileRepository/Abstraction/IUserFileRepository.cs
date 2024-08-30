using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUserFileRepository
{
    Task AddAsync(UserFile userFile);
    Task<IEnumerable<UserFile>> GetAllAsync();
    Task<UserFile> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<string>> GetUserIdsWithAccessToFileAsync(int fileId);
    Task<IEnumerable<UserFile>> GetByFileIdAsync(int fileId);
    Task DeleteByUserIdAsync(Guid userId);
}
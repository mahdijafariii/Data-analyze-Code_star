using AnalysisData.Graph.Model.File;

namespace AnalysisData.Graph.Repository.UserFileRepository.Abstraction;

public interface IUserFileRepository
{
    Task AddAsync(UserFile userFile);
    Task<IEnumerable<UserFile>> GetAllAsync();
    Task<UserFile> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<string>> GetUserIdsAccessHasToFile(int fileId);
    Task<IEnumerable<UserFile>> GetByFileIdAsync(int fileId);
    Task DeleteByUserIdAsync(Guid userId);
}
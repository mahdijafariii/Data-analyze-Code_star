using AnalysisData.Models.GraphModel.File;

namespace AnalysisData.Repositories.GraphRepositories.UserFileRepository.Abstraction;

public interface IUserFileRepository
{
    Task AddAsync(UserFile userFile);
    Task<IEnumerable<UserFile>> GetAllAsync();
    Task<UserFile> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<string>> GetUserIdsAccessHasToFile(int fileId);
    Task<IEnumerable<UserFile>> GetByFileIdAsync(int fileId);
    Task DeleteByUserIdAsync(Guid userId);
}
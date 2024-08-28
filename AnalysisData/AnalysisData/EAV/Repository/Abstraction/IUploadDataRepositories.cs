using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUploadDataRepository
{
    Task<IEnumerable<FileEntity>> GetAllAsync();
    Task<FileEntity> GetByIdAsync(int id);
    Task<IEnumerable<FileEntity>> GetByUserIdAsync(Guid userId);
    Task<int> GetNumberOfFileWithCategoryIdAsync(int categoryId);
    Task AddAsync(FileEntity fileEntity);
}
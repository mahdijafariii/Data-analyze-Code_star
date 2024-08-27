using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUploadDataRepository
{
    Task<IEnumerable<UploadedFile>> GetAllAsync();
    Task<UploadedFile> GetByIdAsync(int id);
    Task<IEnumerable<UploadedFile>> GetByUserIdAsync(Guid userId);
    Task<int> GetNumberOfFileWithCategoryIdAsync(int categoryId);
    Task AddAsync(UploadedFile uploadedFile);
}
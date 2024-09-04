using AnalysisData.Graph.Model.File;

namespace AnalysisData.Graph.Repository.FileUploadedRepository.Abstraction;

public interface IFileUploadedRepository
{
    Task<IEnumerable<FileEntity>> GetUploadedFilesAsync(int page, int limit);
    Task<IEnumerable<FileEntity>> GetAllAsync();
    Task<FileEntity> GetByIdAsync(int id);
    Task<IEnumerable<FileEntity>> GetByUserIdAsync(Guid userId);
    Task<int> GetNumberOfFileWithCategoryIdAsync(int categoryId);
    Task AddAsync(FileEntity fileEntity);
    Task<int> GetTotalFilesCountAsync();
}
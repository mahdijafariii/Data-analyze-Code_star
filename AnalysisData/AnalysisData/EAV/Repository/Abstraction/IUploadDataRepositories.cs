using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUploadDataRepository
{
    Task<IEnumerable<UploadData>> GetAllAsync();
    Task<UploadData> GetByIdAsync(int id);
    Task<IEnumerable<UploadData>> GetByUserIdAsync(Guid userId);
    Task AddAsync(UploadData uploadData);
}
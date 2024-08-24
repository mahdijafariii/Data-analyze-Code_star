using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUploadFileRepository
{
    Task AddAsync(UploadData uploadData);
    Task<IEnumerable<UploadData>> GetAllAsync();
    Task<UploadData> GetByIdAsync(int id);
    Task<UploadData> GetByNameAsync(string name);
    Task DeleteAsync(int id);
    
}
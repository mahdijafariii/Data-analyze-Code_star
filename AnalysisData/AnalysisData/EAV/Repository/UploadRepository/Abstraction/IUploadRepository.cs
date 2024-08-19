using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IUploadRepository
{
    Task AddAsync(Upload entity);
    Task<IEnumerable<Upload>> GetAllAsync();
    Task<Upload> GetByIdAsync(int id);
    Task<Upload> GetByContentAsync(string content);
    Task DeleteAsync(int id);
    
    
}
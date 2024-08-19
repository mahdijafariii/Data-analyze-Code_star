using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IAttributeNodeRepository
{
    Task AddAsync(AttributeNode entity);
    Task<IEnumerable<AttributeNode>> GetAllAsync();
    Task<AttributeNode> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
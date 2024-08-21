using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.NodeRepository.Abstraction;

public interface IAttributeNodeRepository
{
    Task AddAsync(AttributeNode entity);
    Task<IEnumerable<string>> GetTypeOfCategory();
    Task<IEnumerable<AttributeNode>> GetAllAsync();
    Task<AttributeNode> GetByIdAsync(int id);
    Task<AttributeNode> GetByNameAttributeAsync(string name);
    Task DeleteAsync(int id);
}
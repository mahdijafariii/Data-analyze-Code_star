using AnalysisData.Graph.Model.Node;

namespace AnalysisData.Graph.Repository.NodeRepository.Abstraction;

public interface IAttributeNodeRepository
{
    Task AddAsync(AttributeNode entity);
    Task<IEnumerable<AttributeNode>> GetAllAsync();
    Task<AttributeNode> GetByIdAsync(int id);
    Task<AttributeNode> GetByNameAsync(string name);
    Task DeleteAsync(int id);
}
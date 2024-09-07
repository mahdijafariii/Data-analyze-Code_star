using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;

public interface IAttributeNodeRepository
{
    Task AddAsync(AttributeNode entity);
    Task<IEnumerable<AttributeNode>> GetAllAsync();
    Task<AttributeNode> GetByIdAsync(Guid id);
    Task<AttributeNode> GetByNameAsync(string name);
    Task DeleteAsync(Guid id);
}
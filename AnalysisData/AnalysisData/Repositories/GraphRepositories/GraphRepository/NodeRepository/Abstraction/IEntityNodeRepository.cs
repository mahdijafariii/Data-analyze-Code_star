using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;

public interface IEntityNodeRepository
{
    Task AddRangeAsync(IEnumerable<EntityNode> entityNodes); 
    Task AddAsync(EntityNode entity);
    Task<IEnumerable<EntityNode>> GetAllAsync();
    Task<EntityNode> GetByNameAsync(string id);
    Task DeleteAsync(Guid id);
    Task<EntityNode> GetByIdAsync(Guid id);
}
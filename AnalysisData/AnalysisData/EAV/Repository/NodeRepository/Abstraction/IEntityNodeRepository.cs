using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.NodeRepository.Abstraction;

public interface IEntityNodeRepository
{
    Task AddAsync(EntityNode entity);
    Task<IEnumerable<EntityNode>> GetAllAsync();
    Task<EntityNode> GetByNameAsync(string id);
    Task<List<EntityNode>> GetNodesOfEdgeList(IEnumerable<string> nodeIdes);
    Task DeleteAsync(int id);
    Task<EntityNode> GetByIdAsync(int id);

}
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.NodeRepository.Abstraction;

public interface IEntityNodeRepository
{
    Task AddAsync(EntityNode entity);
    Task<IEnumerable<EntityNode>> GetAllAsync();
    Task<EntityNode> GetByEntityNameAsync(string id);
    Task DeleteAsync(int id);
}
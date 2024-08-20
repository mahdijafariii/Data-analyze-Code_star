using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.EdgeRepository.Abstraction;

public interface IEntityEdgeRepository
{
    Task AddAsync(EntityEdge entity);
    Task<IEnumerable<EntityEdge>> GetAllAsync();
    Task<EntityEdge> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
using AnalysisData.Graph.Model.Edge;

namespace AnalysisData.Graph.Repository.EdgeRepository.Abstraction;

public interface IEntityEdgeRepository
{
    Task AddAsync(EntityEdge entity);
    Task AddRangeAsync(IEnumerable<EntityEdge> entityEdges);
    Task<List<EntityEdge>> FindNodeLoopsAsync(int id);
    Task<IEnumerable<EntityEdge>> GetAllAsync();
    Task<EntityEdge> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
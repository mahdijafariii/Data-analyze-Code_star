using AnalysisData.Models.GraphModel.Edge;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;

public interface IEntityEdgeRepository
{
    Task AddAsync(EntityEdge entity);
    Task AddRangeAsync(IEnumerable<EntityEdge> entityEdges);
    Task<List<EntityEdge>> FindNodeLoopsAsync(Guid id);
    Task<IEnumerable<EntityEdge>> GetAllAsync();
    Task<EntityEdge> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
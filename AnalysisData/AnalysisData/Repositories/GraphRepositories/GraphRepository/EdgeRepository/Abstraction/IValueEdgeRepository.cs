using AnalysisData.Models.GraphModel.Edge;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;

public interface IValueEdgeRepository
{
    Task AddAsync(ValueEdge entity);
    Task AddRangeAsync(IEnumerable<ValueEdge> valueEdges);
    Task<IEnumerable<ValueEdge>> GetAllAsync();
    Task<ValueEdge> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
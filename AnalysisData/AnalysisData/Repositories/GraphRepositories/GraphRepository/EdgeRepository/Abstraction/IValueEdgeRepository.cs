using AnalysisData.Models.GraphModel.Edge;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;

public interface IValueEdgeRepository
{
    Task AddAsync(ValueEdge entity);
    Task AddRangeAsync(IEnumerable<ValueEdge> valueEdges);
    Task<IEnumerable<ValueEdge>> GetAllAsync();
    Task<ValueEdge> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
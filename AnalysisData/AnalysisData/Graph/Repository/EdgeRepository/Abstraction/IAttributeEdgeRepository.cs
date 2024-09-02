using AnalysisData.Graph.Model.Edge;

namespace AnalysisData.Graph.Repository.EdgeRepository.Abstraction;

public interface IAttributeEdgeRepository
{
    Task AddAsync(AttributeEdge entity);
    Task<IEnumerable<AttributeEdge>> GetAllAsync();
    Task<AttributeEdge> GetByIdAsync(int id);
    Task<AttributeEdge> GetByNameAsync(string name);

    Task DeleteAsync(int id);
}
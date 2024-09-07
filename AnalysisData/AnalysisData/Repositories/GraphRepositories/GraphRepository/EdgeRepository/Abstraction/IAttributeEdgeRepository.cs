using AnalysisData.Models.GraphModel.Edge;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;

public interface IAttributeEdgeRepository
{
    Task AddAsync(AttributeEdge entity);
    Task AddRangeAsync(IEnumerable<AttributeEdge> attributeEdges);
    Task<IEnumerable<AttributeEdge>> GetAllAsync();
    Task<AttributeEdge> GetByIdAsync(Guid id);
    Task<AttributeEdge> GetByNameAsync(string name);

    Task DeleteAsync(Guid id);
}
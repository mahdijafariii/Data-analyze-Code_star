using AnalysisData.Models.GraphModel.Edge;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.EdgeRepository.Abstraction;

public interface IAttributeEdgeRepository
{
    Task AddAsync(AttributeEdge entity);
    Task AddRangeAsync(IEnumerable<AttributeEdge> attributeEdges);
    Task<IEnumerable<AttributeEdge>> GetAllAsync();
    Task<AttributeEdge> GetByIdAsync(Guid id);
    Task<List<AttributeEdge>> GetByNamesAsync(IEnumerable<string> names);

    Task DeleteAsync(Guid id);
}
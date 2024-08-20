using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.EdgeRepository.Abstraction;

public interface IAttributeEdgeRepository
{
    Task AddAsync(AttributeEdge entity);
    Task<IEnumerable<AttributeEdge>> GetAllAsync();
    Task<AttributeEdge> GetByIdAsync(int id);
    Task<AttributeEdge> GetByNameAttributeAsync(string name);

    Task DeleteAsync(int id);
}
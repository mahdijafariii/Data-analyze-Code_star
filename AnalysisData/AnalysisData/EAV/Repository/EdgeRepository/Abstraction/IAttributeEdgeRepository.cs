using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IAttributeEdgeRepository
{
    Task AddAsync(AttributeEdge entity);
    Task<IEnumerable<AttributeEdge>> GetAllAsync();
    Task<AttributeEdge> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
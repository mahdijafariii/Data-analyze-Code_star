using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository;

public interface IGraphEdgeRepository
{
    IEnumerable<ValueEdge> GetValueEdgeAsync();
    IEnumerable<EntityEdge> GetEntityEdgeAsync();
    Task<IEnumerable<dynamic>> GetEdgeAttributeValues(int id);
}
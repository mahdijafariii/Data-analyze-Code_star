using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IGraphEdgeRepository
{
    IEnumerable<ValueEdge> GetValueEdgeAsync();
    IEnumerable<EntityEdge> GetEntityEdgeAsync();
    Task<bool> IsEdgeAccessibleByUser(string userName, int edgeName);
    Task<IEnumerable<dynamic>> GetEdgeAttributeValues(int id);
}
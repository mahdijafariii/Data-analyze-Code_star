using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository;

public interface IGraphNodeRepository
{
    IEnumerable<ValueNode> GetValueNodesAsync();
    Task<IEnumerable<dynamic>> GetAttributeValues(string headerUniqueId);

    IEnumerable<EntityNode> GetEntityNodesAsync();
}
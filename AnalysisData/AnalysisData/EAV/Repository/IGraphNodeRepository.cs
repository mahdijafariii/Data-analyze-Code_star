using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository;

public interface IGraphNodeRepository
{
    IEnumerable<EntityNode> GetValueNodesAsync();
    Task<IEnumerable<dynamic>> GetAttributeValues(string headerUniqueId);

}
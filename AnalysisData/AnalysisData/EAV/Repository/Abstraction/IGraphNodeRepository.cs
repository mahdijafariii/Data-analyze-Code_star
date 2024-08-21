using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository;

public interface IGraphNodeRepository
{
    Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId);

    IEnumerable<EntityNode> GetEntityNodesAsync();
    IEnumerable<EntityNode> GetEntityNodesWithCategoryAsync(string category);
}
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IGraphNodeRepository
{
    Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId);

    Task<IEnumerable<EntityNode>> GetEntityNodesAsync();
    Task<IEnumerable<EntityNode>> GetEntityNodesWithCategoryAsync(string category);
}
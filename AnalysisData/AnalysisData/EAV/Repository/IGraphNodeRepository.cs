using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository;

public interface IGraphNodeRepository
{
    IEnumerable<EntityNode> GetEntityNodesAsync();
    IEnumerable<EntityNode> GetEntityNodesWithCategoryAsync(string category);
}
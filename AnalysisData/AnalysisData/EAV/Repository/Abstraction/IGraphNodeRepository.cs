using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IGraphNodeRepository
{
    Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId);

    Task<IEnumerable<EntityNode>> GetEntityNodesAsync();
    Task<IEnumerable<EntityNode>> GetNodeContainSearchInput(string input);
    Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInput(string input);
    Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInput(string input);
    Task<IEnumerable<EntityNode>> GetEntityNodesWithCategoryIdAsync(int categoryId);
}
using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IGraphNodeRepository
{
    Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId);

    Task<IEnumerable<EntityNode>> GetEntityNodesAsAdminAsync();
    Task<IEnumerable<EntityNode>> GetNodeContainSearchInput(string input);
    Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInput(string input);
    Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInput(string input);
    Task<IEnumerable<EntityNode>> GetEntityAsAdminNodesWithCategoryIdAsync(int categoryId);
    Task<IEnumerable<EntityNode>> GetEntityAsUserNodesWithCategoryIdAsync(string userGuidId, int categoryId);
    Task<IEnumerable<EntityNode>> GetEntityNodesAsUserAsync(string userGuidId);
}
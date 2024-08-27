using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IGraphNodeRepository
{
    Task<IEnumerable<dynamic>> GetNodeAttributeValue(string headerUniqueId);
    Task<bool> IsNodeAccessibleByUser(string userName, string nodeName);
    Task<IEnumerable<EntityNode>> GetEntityNodesAsAdminAsync();
    Task<IEnumerable<EntityNode>> GetNodeContainSearchInputAsAdmin(string input);
    Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputAsAdmin(string input);
    Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputAsAdmin(string input);
    Task<IEnumerable<EntityNode>> GetEntityAsAdminNodesWithCategoryIdAsync(int categoryId);
    Task<IEnumerable<EntityNode>> GetEntityAsUserNodesWithCategoryIdAsync(string userGuidId, int categoryId);
    Task<IEnumerable<EntityNode>> GetEntityNodesAsUserAsync(string userGuidId);
    Task<IEnumerable<EntityNode>> GetNodeContainSearchInputAsUser(string username, string input);
    Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputAsUser(string username, string input);
    Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputAsUser(string username, string input);

}
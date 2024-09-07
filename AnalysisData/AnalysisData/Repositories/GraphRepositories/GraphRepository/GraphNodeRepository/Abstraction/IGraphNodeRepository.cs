using AnalysisData.Dtos.GraphDto.NodeDto;
using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphNodeRepository.Abstraction;

public interface IGraphNodeRepository
{
    Task<IEnumerable<NodeInformationDto>> GetNodeAttributeValueAsync(int id);
    Task<bool> IsNodeAccessibleByUser(Guid userName, int nodeId);
    Task<IEnumerable<EntityNode>> GetEntityNodesForAdminAsync();
    Task<IEnumerable<EntityNode>> GetNodeContainSearchInputForAdminAsync(string input);
    Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputForAdminAsync(string input);
    Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputForAdminAsync(string input);
    Task<IEnumerable<EntityNode>> GetEntityNodesForAdminWithCategoryIdAsync(int categoryId);
    Task<IEnumerable<EntityNode>> GetEntityNodeForUserWithCategoryIdAsync(Guid userGuidId, int categoryId);
    Task<IEnumerable<EntityNode>> GetEntityNodesForUserAsync(Guid userGuidId);
    Task<IEnumerable<EntityNode>> GetNodeContainSearchInputForUserAsync(Guid username, string input);
    Task<IEnumerable<EntityNode>> GetNodeStartsWithSearchInputForUserAsync(Guid username, string input);
    Task<IEnumerable<EntityNode>> GetNodeEndsWithSearchInputForUserAsync(Guid username, string input);
}
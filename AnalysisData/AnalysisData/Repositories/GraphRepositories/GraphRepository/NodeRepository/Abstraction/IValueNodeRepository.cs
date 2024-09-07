using AnalysisData.Models.GraphModel.Node;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.NodeRepository.Abstraction;

public interface IValueNodeRepository
{
    Task AddRangeAsync(IEnumerable<ValueNode> valueNodes);
    Task AddAsync(ValueNode entity);
    Task<IEnumerable<ValueNode>> GetAllAsync();
    Task<ValueNode> GetByIdAsync(Guid id);
    Task DeleteAsync(Guid id);
}
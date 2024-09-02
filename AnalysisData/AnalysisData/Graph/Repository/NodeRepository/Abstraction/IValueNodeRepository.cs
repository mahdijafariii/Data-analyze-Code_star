using AnalysisData.Graph.Model.Node;

namespace AnalysisData.Graph.Repository.NodeRepository.Abstraction;

public interface IValueNodeRepository
{
    Task AddAsync(ValueNode entity);
    Task<IEnumerable<ValueNode>> GetAllAsync();
    Task<ValueNode> GetByIdAsync(int id);
    Task DeleteAsync(int id);
}
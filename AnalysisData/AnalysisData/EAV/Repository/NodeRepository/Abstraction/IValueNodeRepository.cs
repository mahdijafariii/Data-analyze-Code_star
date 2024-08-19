using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository.Abstraction;

public interface IValueNodeRepository
{
    Task AddAsync(ValueNode entity);
    Task<IEnumerable<ValueNode>> GetAllAsync();
    Task<ValueNode> GetByIdAsync(int id);
    Task DeleteAsync(object id);
}
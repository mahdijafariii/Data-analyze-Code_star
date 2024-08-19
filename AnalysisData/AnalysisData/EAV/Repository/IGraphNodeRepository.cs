using AnalysisData.EAV.Model;

namespace AnalysisData.EAV.Repository;

public interface IGraphNodeRepository
{
    Task<IEnumerable<ValueNode>> GetValueNodesByAttributeAsync(params string[] attributeNames);
}
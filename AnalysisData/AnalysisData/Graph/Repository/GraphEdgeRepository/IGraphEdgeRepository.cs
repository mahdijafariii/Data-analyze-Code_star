using AnalysisData.Graph.Dto.EdgeDto;

namespace AnalysisData.Graph.Repository.GraphEdgeRepository;

public interface IGraphEdgeRepository
{
    Task<bool> IsEdgeAccessibleByUser(string userName, int edgeName);
    Task<IEnumerable<EdgeInformationDto>> GetEdgeAttributeValues(int id);
}
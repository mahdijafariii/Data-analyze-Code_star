using AnalysisData.Dtos.GraphDto.EdgeDto;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository.Abstraction;

public interface IGraphEdgeRepository
{
    Task<bool> IsEdgeAccessibleByUser(string userName, int edgeName);
    Task<IEnumerable<EdgeInformationDto>> GetEdgeAttributeValues(int id);
}
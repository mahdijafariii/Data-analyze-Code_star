using AnalysisData.Dtos.GraphDto.EdgeDto;

namespace AnalysisData.Repositories.GraphRepositories.GraphRepository.GraphEdgeRepository.Abstraction;

public interface IGraphEdgeRepository
{
    Task<bool> IsEdgeAccessibleByUser(string userName, Guid edgeName);
    Task<IEnumerable<EdgeInformationDto>> GetEdgeAttributeValues(Guid id);
}
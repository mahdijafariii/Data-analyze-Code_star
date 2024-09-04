namespace AnalysisData.Graph.Repository.GraphEdgeRepository;

public interface IGraphEdgeRepository
{
    Task<bool> IsEdgeAccessibleByUser(string userName, int edgeName);
    Task<IEnumerable<dynamic>> GetEdgeAttributeValues(int id);
}
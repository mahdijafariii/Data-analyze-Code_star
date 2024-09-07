using AnalysisData.Graph.Model.Node;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface INodeRecordProcessor
{
    Task<IEnumerable<EntityNode>> ProcessEntityNodesAsync(ICsvReader csv, IEnumerable<string> headers, string id,
        int fileId);
}
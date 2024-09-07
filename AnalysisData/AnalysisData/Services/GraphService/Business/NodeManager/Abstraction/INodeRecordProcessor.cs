using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

public interface INodeRecordProcessor
{
    Task<IEnumerable<EntityNode>> ProcessEntityNodesAsync(ICsvReaderProcessor csv, IEnumerable<string> headers, string id,
        int fileId);
}
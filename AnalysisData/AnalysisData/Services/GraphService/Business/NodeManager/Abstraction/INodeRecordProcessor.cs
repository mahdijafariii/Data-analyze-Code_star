using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

public interface INodeRecordProcessor
{
    Task ProcessNodesAsync(ICsvReaderProcessor csv, IEnumerable<AttributeNode> headers, string id, int fileId);
}
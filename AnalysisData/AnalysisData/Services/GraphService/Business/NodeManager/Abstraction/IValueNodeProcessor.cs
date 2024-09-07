using AnalysisData.Models.GraphModel.Node;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.NodeManager.Abstraction;

public interface IValueNodeProcessor
{
    Task ProcessValueNodesAsync(ICsvReaderProcessor csv, IEnumerable<EntityNode> items, IEnumerable<string> headers, string id);
}
using AnalysisData.Graph.Model.Node;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IValueNodeProcessor
{
    Task ProcessValueNodesAsync(CsvReader csv, IEnumerable<EntityNode> items, IEnumerable<string> headers, string id);
}
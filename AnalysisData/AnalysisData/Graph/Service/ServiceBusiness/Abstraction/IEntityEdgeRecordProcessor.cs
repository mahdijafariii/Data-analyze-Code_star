using AnalysisData.Graph.Model.Edge;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IEntityEdgeRecordProcessor
{
    Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(string from, string to);
}
using AnalysisData.Graph.Model.Edge;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IEntityEdgeRecordProcessor
{
    Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(ICsvReader csv, string fromId, string toId);
}
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

public interface IEntityEdgeRecordProcessor
{
    Task<IEnumerable<EntityEdge>> ProcessEntityEdgesAsync(ICsvReaderProcessor csv, string fromId, string toId);
}
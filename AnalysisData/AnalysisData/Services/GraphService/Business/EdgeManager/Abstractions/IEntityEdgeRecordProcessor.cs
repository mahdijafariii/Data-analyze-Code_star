using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

public interface IEntityEdgeRecordProcessor
{
    Task ProcessEdgesAsync(ICsvReaderProcessor csv, IEnumerable<AttributeEdge> attributeEdges, string from,
        string to);
}
using AnalysisData.Models.GraphModel.Edge;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

public interface IValueEdgeProcessor
{
    Task ProcessEntityEdgeValuesAsync(
        ICsvReaderProcessor csv, 
        IEnumerable<string> headers, 
        string from, 
        string to, 
        IEnumerable<EntityEdge> entityEdges);
}
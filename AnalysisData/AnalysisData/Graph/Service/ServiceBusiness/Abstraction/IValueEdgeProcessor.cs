using AnalysisData.Graph.Model.Edge;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IValueEdgeProcessor
{
    Task ProcessEntityEdgeValuesAsync(
        ICsvReader csv, 
        IEnumerable<string> headers, 
        string from, 
        string to, 
        IEnumerable<EntityEdge> entityEdges);
}
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IEdgeRecordProcessor
{
    Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string from, string to);
}
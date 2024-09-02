using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface INodeRecordProcessor
{
    Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string id, int fileId);
}
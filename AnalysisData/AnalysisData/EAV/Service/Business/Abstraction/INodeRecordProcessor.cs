using CsvHelper;

namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface INodeRecordProcessor
{
    Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string id, string category);
}
using CsvHelper;

namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface IEdgeRecordProcessor
{
    Task ProcessRecordsAsync(CsvReader csv, IEnumerable<string> headers, string from, string to);
}
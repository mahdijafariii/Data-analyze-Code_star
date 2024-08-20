using CsvHelper;

namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface INodeRecordProcessor
{
    Task ProcessRecordsAsync(CsvReader csv, string[] headers, string id);
}
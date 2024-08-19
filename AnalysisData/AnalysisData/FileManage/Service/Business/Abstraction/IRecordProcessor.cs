using AnalysisData.EAV.Model;
using CsvHelper;

namespace AnalysisData.FileManage.Service.Business;

public interface IRecordProcessor
{
    Task ProcessRecordsAsync(CsvReader csv, string[] headers, string id);
}
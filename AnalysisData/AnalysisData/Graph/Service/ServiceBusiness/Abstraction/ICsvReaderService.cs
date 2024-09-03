using CsvHelper;

namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface ICsvReaderService
{
    CsvReader CreateCsvReader(IFormFile file);
    IEnumerable<string> ReadHeaders(CsvReader csv, List<string> requiredHeaders);
}
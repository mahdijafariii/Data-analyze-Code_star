namespace AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

public interface ICsvReaderManager
{
    ICsvReaderProcessor CreateCsvReader(IFormFile file);
    IEnumerable<string> ReadHeaders(ICsvReaderProcessor csv, List<string> requiredHeaders);
}
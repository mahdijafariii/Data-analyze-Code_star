using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReaderService
{
    CsvReader CreateCsvReader(IFormFile file);
    IEnumerable<string> ReadHeaders(CsvReader csv, List<string> requiredHeaders);
}
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReaderService
{
    ICsvReader CreateCsvReader(IFormFile file);
    IEnumerable<string> ReadHeaders(ICsvReader csv, List<string> requiredHeaders);
}
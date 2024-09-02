using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReaderService
{
    public CsvReader CreateCsvReader(IFormFile file);
    public IEnumerable<string> ReadHeaders(CsvReader csv);
}
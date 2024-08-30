using CsvHelper;

namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface ICsvReaderService
{
    public CsvReader CreateCsvReader(IFormFile file);
    public IEnumerable<string> ReadHeaders(CsvReader csv);
}
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

namespace AnalysisData.Services.GraphService.Business.CsvManager;

public class CsvHeaderReaderProcessor : ICsvHeaderReaderProcessor
{
    public IEnumerable<string> ReadHeaders(ICsvReaderProcessor csv)
    {
        if (csv.Read())
        {
            csv.ReadHeader();
            return csv.HeaderRecord ?? Enumerable.Empty<string>();
        }

        return Enumerable.Empty<string>();
    }
}
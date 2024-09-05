using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvHeaderReader : ICsvHeaderReader
{
    public IEnumerable<string> ReadHeaders(ICsvReader csv)
    {
        if (csv.Read())
        {
            csv.ReadHeader();
            return csv.HeaderRecord ?? Enumerable.Empty<string>();
        }

        return Enumerable.Empty<string>();
    }
}
using System.Globalization;
using System.Text;
using AnalysisData.EAV.Service.Business.Abstraction;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalysisData.EAV.Service.Business;

public class CsvReaderService : ICsvReaderService
{
    public CsvReader CreateCsvReader(IFormFile file)
    {
        var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            HasHeaderRecord = true
        };
        return new CsvReader(reader, config);
    }

    public IEnumerable<string> ReadHeaders(CsvReader csv)
    {
        if (csv.Read())
        {
            csv.ReadHeader();
            return csv.Context.Reader.HeaderRecord ?? Enumerable.Empty<string>();
        }
        return Enumerable.Empty<string>();
    }
}
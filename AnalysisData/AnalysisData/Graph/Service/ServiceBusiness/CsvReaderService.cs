using System.Globalization;
using System.Text;
using AnalysisData.EAV.Service.Business.Abstraction;
using AnalysisData.Exception;
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

    public IEnumerable<string> ReadHeaders(CsvReader csv, List<string> requiredHeaders)
    {
        if (csv.Read())
        {
            csv.ReadHeader();
            var headers = csv.Context.Reader.HeaderRecord ?? Enumerable.Empty<string>();
            
            var missingHeaders = requiredHeaders.Where(h => !headers.Contains(h)).ToList();
            if (missingHeaders.Any())
            {
                throw new HeaderIdNotFoundInNodeFile(missingHeaders);
            }

            return headers;
        }

        return Enumerable.Empty<string>();
    }
}
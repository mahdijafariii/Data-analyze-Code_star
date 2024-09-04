using System.Globalization;
using System.Text;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvReaderFactory : ICsvReaderFactory
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
}
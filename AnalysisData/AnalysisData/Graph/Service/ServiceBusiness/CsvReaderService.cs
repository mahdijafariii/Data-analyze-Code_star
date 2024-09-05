using System.Globalization;
using System.Text;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvReaderService : ICsvReaderService
{
    private readonly ICsvHeaderReader _csvHeaderReader;
    private readonly IHeaderValidator _headerValidator;

    public CsvReaderService(ICsvHeaderReader csvHeaderReader, IHeaderValidator headerValidator)
    {
        _csvHeaderReader = csvHeaderReader;
        _headerValidator = headerValidator;
    }

    public ICsvReader CreateCsvReader(IFormFile file)
    {
        var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            HasHeaderRecord = true
        };
        var csvHelperReader = new CsvHelper.CsvReader(reader, config);
        return new CsvReaderWrapper(csvHelperReader);
    }

    public IEnumerable<string> ReadHeaders(ICsvReader csv, List<string> requiredHeaders)
    {
        var headers = _csvHeaderReader.ReadHeaders(csv);
        _headerValidator.ValidateHeaders(headers, requiredHeaders);
        return headers;
    }
}
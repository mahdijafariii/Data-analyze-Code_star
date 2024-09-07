using System.Globalization;
using System.Text;
using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;
using CsvHelper.Configuration;

namespace AnalysisData.Services.GraphService.Business.CsvManager;

public class CsvReaderManager : ICsvReaderManager
{
    private readonly ICsvHeaderReaderProcessor _csvHeaderReaderProcessor;
    private readonly IHeaderValidatorProcessor _headerValidatorProcessor;

    public CsvReaderManager(ICsvHeaderReaderProcessor csvHeaderReaderProcessor, IHeaderValidatorProcessor headerValidatorProcessor)
    {
        _csvHeaderReaderProcessor = csvHeaderReaderProcessor;
        _headerValidatorProcessor = headerValidatorProcessor;
    }

    public ICsvReaderProcessor CreateCsvReader(IFormFile file)
    {
        var reader = new StreamReader(file.OpenReadStream());
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Encoding = Encoding.UTF8,
            HasHeaderRecord = true
        };
        var csvHelperReader = new CsvHelper.CsvReader(reader, config);
        return new CsvReaderProcessorProcessor(csvHelperReader);
    }

    public IEnumerable<string> ReadHeaders(ICsvReaderProcessor csv, List<string> requiredHeaders)
    {
        var headers = _csvHeaderReaderProcessor.ReadHeaders(csv);
        _headerValidatorProcessor.ValidateHeaders(headers, requiredHeaders);
        return headers;
    }
}
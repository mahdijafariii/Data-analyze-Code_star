using System.Globalization;
using System.Text;
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using AnalysisData.Exception;
using AnalysisData.Exception.GraphException;
using CsvHelper;
using CsvHelper.Configuration;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvReaderService : ICsvReaderService
{
    private readonly ICsvReaderFactory _csvReaderFactory;
    private readonly ICsvHeaderValidator _csvHeaderValidator;

    public CsvReaderService(ICsvReaderFactory csvReaderFactory, ICsvHeaderValidator csvHeaderValidator)
    {
        _csvReaderFactory = csvReaderFactory;
        _csvHeaderValidator = csvHeaderValidator;
    }

    public IEnumerable<string> ValidateCsvHeaders(IFormFile file, List<string> requiredHeaders)
    {
        var csvReader = _csvReaderFactory.CreateCsvReader(file);
        return _csvHeaderValidator.ReadAndValidateHeaders(csvReader, requiredHeaders);
    }
}
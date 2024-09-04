using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReaderService
{
    IEnumerable<string> ValidateCsvHeaders(IFormFile file, List<string> requiredHeaders);
}
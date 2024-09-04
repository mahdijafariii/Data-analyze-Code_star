using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvHeaderValidator
{
    IEnumerable<string> ReadAndValidateHeaders(CsvReader csv, List<string> requiredHeaders);
}
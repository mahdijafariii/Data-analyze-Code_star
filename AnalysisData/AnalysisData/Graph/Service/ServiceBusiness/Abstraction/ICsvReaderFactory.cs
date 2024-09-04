using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface ICsvReaderFactory
{
    CsvReader CreateCsvReader(IFormFile file);
}
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvReaderAdapter : ICsvReader
{
    private readonly CsvReader _csvReader;

    public CsvReaderAdapter(CsvReader csvReader)
    {
        _csvReader = csvReader;
    }

    public bool Read() => _csvReader.Read();
    public string GetField(string name) => _csvReader.GetField(name);
}
using AnalysisData.Graph.Service.ServiceBusiness.Abstraction;
using CsvHelper;

namespace AnalysisData.Graph.Service.ServiceBusiness;

public class CsvReaderWrapper : ICsvReader
{
    private readonly CsvReader _csvReader;

    public CsvReaderWrapper(CsvReader csvReader)
    {
        _csvReader = csvReader ?? throw new ArgumentNullException(nameof(csvReader));
    }

    public bool Read() => _csvReader.Read();
    public void ReadHeader() => _csvReader.ReadHeader();
    public string[] HeaderRecord => _csvReader.HeaderRecord;
    public string GetField(string fieldName) => _csvReader.GetField(fieldName);
}
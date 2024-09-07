using AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;
using CsvHelper;

namespace AnalysisData.Services.GraphService.Business.CsvManager;

public class CsvReaderProcessorProcessor : ICsvReaderProcessor
{
    private readonly CsvReader _csvReader;

    public CsvReaderProcessorProcessor(CsvReader csvReader)
    {
        _csvReader = csvReader ?? throw new ArgumentNullException(nameof(csvReader));
    }

    public bool Read() => _csvReader.Read();
    public void ReadHeader() => _csvReader.ReadHeader();
    public string[] HeaderRecord => _csvReader.HeaderRecord;
    public string GetField(string fieldName) => _csvReader.GetField(fieldName);
}
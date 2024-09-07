namespace AnalysisData.Services.GraphService.Business.CsvManager.Abstractions;

public interface ICsvHeaderReaderProcessor
{
    IEnumerable<string> ReadHeaders(ICsvReaderProcessor csv);
}
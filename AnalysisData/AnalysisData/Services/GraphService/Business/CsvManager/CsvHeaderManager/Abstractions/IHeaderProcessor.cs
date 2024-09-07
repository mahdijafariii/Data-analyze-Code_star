namespace AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;

public interface IHeaderProcessor
{
    Task ProcessHeadersAsync(IEnumerable<string> headers, string uniqueAttribute);
}
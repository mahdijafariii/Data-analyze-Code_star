namespace AnalysisData.Services.GraphService.Business.CsvManager.CsvHeaderManager.Abstractions;

public interface IHeaderValidatorProcessor
{
    void ValidateHeaders(IEnumerable<string> headers, List<string> requiredHeaders);
}
namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface IHeaderProcessor
{
    Task ProcessHeadersAsync(IEnumerable<string> headers, string uniqueAttribute);
}
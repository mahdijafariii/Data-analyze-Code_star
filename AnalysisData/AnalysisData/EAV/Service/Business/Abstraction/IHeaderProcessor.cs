namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface IHeaderProcessor
{
    Task ProcessHeadersAsync(string[] headers, string uniqueAttribute);
}
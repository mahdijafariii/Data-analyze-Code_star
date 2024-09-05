namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IHeaderProcessor
{
    Task ProcessHeadersAsync(IEnumerable<string> headers, string uniqueAttribute);
}
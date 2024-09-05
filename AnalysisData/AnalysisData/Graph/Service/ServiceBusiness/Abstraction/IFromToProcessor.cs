namespace AnalysisData.Graph.Service.ServiceBusiness.Abstraction;

public interface IFromToProcessor
{
    Task ProcessFromToAsync(IEnumerable<string> headers, string from, string to);
}
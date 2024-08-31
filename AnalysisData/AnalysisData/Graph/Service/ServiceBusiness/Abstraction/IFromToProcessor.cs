namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface IFromToProcessor
{
    Task ProcessFromToAsync(IEnumerable<string> headers, string from, string to);
}
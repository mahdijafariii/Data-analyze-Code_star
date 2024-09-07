namespace AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

public interface IFromToProcessor
{
    Task ProcessFromToAsync(IEnumerable<string> headers, string from, string to);
}
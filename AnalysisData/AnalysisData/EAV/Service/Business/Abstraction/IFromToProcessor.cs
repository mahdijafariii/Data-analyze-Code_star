namespace AnalysisData.EAV.Service.Business.Abstraction;

public interface IFromToProcessor
{
    Task ProcessFromToAsync(string[] headers, string from, string to);
}
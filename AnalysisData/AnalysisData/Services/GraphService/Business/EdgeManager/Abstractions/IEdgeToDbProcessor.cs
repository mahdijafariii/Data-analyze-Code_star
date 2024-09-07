namespace AnalysisData.Services.GraphService.Business.EdgeManager.Abstractions;

public interface IEdgeToDbProcessor
{
    Task ProcessCsvFileAsync(IFormFile file, string from, string to);
}
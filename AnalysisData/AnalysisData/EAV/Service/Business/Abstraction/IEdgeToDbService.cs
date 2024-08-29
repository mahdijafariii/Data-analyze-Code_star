namespace AnalysisData.EAV.Service.Abstraction;

public interface IEdgeToDbService
{
    Task ProcessCsvFileAsync(IFormFile file, string from, string to);
}
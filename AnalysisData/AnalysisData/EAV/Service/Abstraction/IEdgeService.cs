namespace AnalysisData.EAV.Service.Abstraction;

public interface IEdgeService
{
    Task ProcessCsvFileAsync(IFormFile file, string from,string to);

}
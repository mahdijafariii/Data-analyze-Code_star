namespace AnalysisData.EAV.Service.Abstraction;

public interface IEdge2DBService
{
    Task ProcessCsvFileAsync(IFormFile file, string from,string to);

}
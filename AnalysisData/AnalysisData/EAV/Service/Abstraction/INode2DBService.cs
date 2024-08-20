namespace AnalysisData.EAV.Service.Abstraction;

public interface INode2DBService
{
    Task ProcessCsvFileAsync(IFormFile file, string id, string fileName);
}
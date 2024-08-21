namespace AnalysisData.EAV.Service.Abstraction;

public interface INodeToDbService
{
    Task ProcessCsvFileAsync(IFormFile file, string id, string fileName);
}